using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Factura;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Factura;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IFacturaService<
            FacturaResponseDTO,
            FacturaListDTO,
            FacturaCreateDTO> _facturaService;
        private readonly IGenericServices<
            ClienteResponseDTO,
            ClienteCreateDTO,
            ClienteUpdateDTO> _clienteService;
        private readonly IGenericServices<
            ProductoResponseDTO,
            ProductoCreateDTO,
            ProductoUpdateDTO> _productoService;
        private readonly IMapper _mapper;

        public FacturaController(
            IFacturaService<
                FacturaResponseDTO,
                FacturaListDTO,
                FacturaCreateDTO> facturaService,
            IGenericServices<
                ClienteResponseDTO,
                ClienteCreateDTO,
                ClienteUpdateDTO> clienteService,
            IGenericServices<
                ProductoResponseDTO,
                ProductoCreateDTO,
                ProductoUpdateDTO> productoService,
            IMapper mapper)
        {
            _facturaService = facturaService;
            _clienteService = clienteService;
            _productoService = productoService;
            _mapper = mapper;
        }

        // GET: FacturaController
        public async Task<ActionResult> Index()
        {
            try
            {
                var response = await _facturaService.ObtenerTodosAsync();
                var facturas = _mapper.Map<List<FacturaIndexViewModel>>(response.Data);

                return View(facturas);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return View(new List<FacturaIndexViewModel>());
            }
        }

        // GET: FacturaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdAsync(id);
                var factura = _mapper.Map<FacturaDetailsViewModel>(response.Data);

                return View(factura.TipoDocumento == inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica
                    ? "DetailsNotaCredito" : "Details", factura);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: FacturaController/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var facturaVM = new FacturaCreateViewModel();

            return View(await CargarListasAsync(facturaVM));
        }

        [HttpGet]
        public async Task<ActionResult> NotaCredito(int id)
        {
            var response = await _facturaService.ObtenerPorIdAsync(id);
            if (response.Data.TipoDocumento != inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica || !response.Data.Estado)
            {
                TempData["Mensaje"] = "Solo se puede generar una nota de crédito desde una factura electrónica activa.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new FacturaCreateViewModel
            {
                TipoDocumento = inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica,
                FacturaOrigenId = response.Data.Id,
                NumeroDocumentoOriginal = response.Data.NumeroFactura,
                ClienteId = response.Data.ClienteId,
                ClienteSeleccionado = response.Data.Cliente,
                Detalles = response.Data.Detalles
                    .Where(d => d.CantidadDisponibleAcreditar > 0)
                    .Select(d => new FacturaDetalleViewModel
                    {
                        ProductoId = d.ProductoId,
                        Producto = d.Producto,
                        Cantidad = d.CantidadDisponibleAcreditar,
                        CantidadMaxima = d.CantidadDisponibleAcreditar,
                        PrecioUnitario = d.PrecioUnitario,
                        PorcentajeImpuesto = d.PorcentajeImpuesto,
                        PorcentajeDescuento = d.PorcentajeDescuento
                    }).ToList()
            };
            return View("NotaCredito", await CargarListasAsync(vm));
        }

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FacturaCreateViewModel facturaVM)
        {
            facturaVM.TipoDocumento = facturaVM.FacturaOrigenId.HasValue
                ? inaApp.Common.Enums.Enums.TipoDocumento.NotaCreditoElectronica
                : inaApp.Common.Enums.Enums.TipoDocumento.FacturaElectronica;

            if (facturaVM.Detalles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un producto.");
            }

            if (!ModelState.IsValid)
            {
                CalcularTotales(facturaVM);
                return View(facturaVM.EsNotaCredito ? "NotaCredito" : "Create",
                    await CargarListasAsync(facturaVM));
            }

            try
            {
                var facturaDTO = _mapper.Map<FacturaCreateDTO>(facturaVM);
                var response = await _facturaService.CrearAsync(facturaDTO);

                if (!response.Success || response.Data == null)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    CalcularTotales(facturaVM);
                    return View(facturaVM.EsNotaCredito ? "NotaCredito" : "Create",
                         await CargarListasAsync(facturaVM));
                }

                TempData["Mensaje"] = response.Message;
                return RedirectToAction(nameof(Details), new { id = response.Data.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                CalcularTotales(facturaVM);
                return View(facturaVM.EsNotaCredito ? "NotaCredito" : "Create",
                    await CargarListasAsync(facturaVM));
            }
        }

        // Agrega un detalle mediante un envío normal del formulario. De esta forma,
        // la construcción de la factura no depende de JavaScript en el navegador.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarProducto(FacturaCreateViewModel facturaVM)
        {
            var facturaConListas = await CargarListasAsync(facturaVM);
            var producto = facturaConListas.ProductosDisponibles
                .SingleOrDefault(p => p.Id == facturaVM.ProductoSeleccionadoId);
            var cantidad = facturaVM.Cantidad.GetValueOrDefault();

            ModelState.Clear();

            if (producto == null || cantidad < 1)
            {
                ModelState.AddModelError(string.Empty,
                    "Seleccione un producto e indique una cantidad válida.");
            }
            else if (cantidad > producto.Stock)
            {
                ModelState.AddModelError(string.Empty,
                    "No hay stock suficiente para la cantidad indicada.");
            }
            else if (facturaVM.Detalles.Any(d => d.ProductoId == producto.Id))
            {
                ModelState.AddModelError(string.Empty,
                    "El producto ya fue agregado a la factura.");
            }
            else
            {
                facturaVM.Detalles.Add(new FacturaDetalleViewModel
                {
                    ProductoId = producto.Id,
                    Producto = producto.Nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio,
                    PorcentajeImpuesto = producto.PorcentajeImpuesto
                });

                facturaVM.ProductoSeleccionadoId = null;
                facturaVM.Cantidad = null;
            }

            CalcularTotales(facturaVM);
            return View("Create", facturaConListas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EliminarProducto(
            FacturaCreateViewModel facturaVM,
            int indice)
        {
            ModelState.Clear();

            if (indice >= 0 && indice < facturaVM.Detalles.Count)
            {
                facturaVM.Detalles.RemoveAt(indice);
            }

            CalcularTotales(facturaVM);
            return View("Create", await CargarListasAsync(facturaVM));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActualizarTotales(FacturaCreateViewModel facturaVM)
        {
            ModelState.Clear();
            CalcularTotales(facturaVM);
            return View("Create", await CargarListasAsync(facturaVM));
        }


        // GET: FacturaController/Edit/5
        /*public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdAsync(id);
                var factura = _mapper.Map<FacturaDetailsViewModel>(response.Data);

                return View("Details", factura);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }


        // POST: FacturaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            TempData["Mensaje"] = "Las facturas confirmadas no se pueden editar.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: FacturaController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Anular(id);
        }
        */

        // Una factura electrónica emitida se modifica exclusivamente generando
        // una Nota de Crédito; Edit nunca presenta un formulario modificable.
        public Task<ActionResult> Edit(int id) => NotaCredito(id);


        private void CalcularTotales(FacturaCreateViewModel facturaVM)
        {
            var facturaDTO = _mapper.Map<FacturaCreateDTO>(facturaVM);
            var facturaCalculada = _facturaService.CalcularTotales(facturaDTO);

            for (var indice = 0; indice < facturaVM.Detalles.Count; indice++)
            {
                facturaVM.Detalles[indice].Descuento = facturaCalculada.Detalles[indice].Descuento;
                facturaVM.Detalles[indice].Subtotal = facturaCalculada.Detalles[indice].Subtotal;
                facturaVM.Detalles[indice].Impuesto = facturaCalculada.Detalles[indice].Impuesto;
                facturaVM.Detalles[indice].TotalLinea = facturaCalculada.Detalles[indice].TotalLinea;
            }

            facturaVM.Subtotal = facturaCalculada.Subtotal;
            facturaVM.Impuesto = facturaCalculada.Impuesto;
            facturaVM.Total = facturaCalculada.Total;
        }


        // POST: FacturaController/Delete/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            return await ConfirmarAnulacion(id);
        }

        // GET: FacturaController/Anular/5
        [HttpGet]
        public async Task<ActionResult> Anular(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdAsync(id);
                var factura = _mapper.Map<FacturaDetailsViewModel>(response.Data);

                return View(factura);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: FacturaController/Anular/5
        [HttpPost]
        [ActionName("Anular")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmarAnulacion(int id)
        {
            try
            {
                var response = await _facturaService.AnularAsync(id);
                TempData["Mensaje"] = response.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }*/

        private async Task<FacturaCreateViewModel> CargarListasAsync(
            FacturaCreateViewModel facturaVM)
        {
            var clientesResponse = await _clienteService.ObtenerTodosAsync();
            var productosResponse = await _productoService.ObtenerTodosAsync();
            var clientes = clientesResponse.Data ?? new List<ClienteResponseDTO>();
            var productos = productosResponse.Data ?? new List<ProductoResponseDTO>();

            facturaVM.ClientesDisponibles = clientes.Where(c => c.Estado).Select(c => new ClienteDisponibleViewModel { Id = c.IdCliente, Identificacion = c.NumeroIdentificacion, Nombre = $"{c.Nombre} {c.Apellido1}" }).ToList();

            facturaVM.Clientes = clientes.Select(cliente => new SelectListItem
            {
                Value = cliente.IdCliente.ToString(),
                Text = $"{cliente.Nombre} {cliente.Apellido1}",
                Selected = cliente.IdCliente == facturaVM.ClienteId
            }).ToList();

            facturaVM.Productos = productos.Select(producto => new SelectListItem
            {
                Value = producto.Id.ToString(),
                Text = producto.Nombre
            }).ToList();

            facturaVM.ProductosDisponibles = productos.Select(producto =>
                new ProductoDisponibleViewModel
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Stock = producto.Stock
                }).ToList();

            return facturaVM;
        }
    }
}