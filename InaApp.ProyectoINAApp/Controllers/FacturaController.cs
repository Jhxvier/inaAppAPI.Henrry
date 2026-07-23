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
        private const decimal PorcentajeImpuesto = 0.13m;

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

                return View(factura);
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

        // POST: FacturaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FacturaCreateViewModel facturaVM)
        {
            if (facturaVM.Detalles.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Debe agregar al menos un producto.");
            }

            if (!ModelState.IsValid)
            {
                return View(await CargarListasAsync(facturaVM));
            }

            try
            {
                CalcularTotales(facturaVM);

                var facturaDTO = _mapper.Map<FacturaCreateDTO>(facturaVM);
                var response = await _facturaService.CrearAsync(facturaDTO);

                TempData["Mensaje"] = response.Message;
                return RedirectToAction(nameof(Details), new { id = response.Data.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(await CargarListasAsync(facturaVM));
            }
        }

        // GET: FacturaController/Edit/5
        public async Task<ActionResult> Edit(int id)
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

        // POST: FacturaController/Delete/5
        [HttpPost]
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
        }

        private async Task<FacturaCreateViewModel> CargarListasAsync(
            FacturaCreateViewModel facturaVM)
        {
            var clientesResponse = await _clienteService.ObtenerTodosAsync();
            var productosResponse = await _productoService.ObtenerTodosAsync();
            var clientes = clientesResponse.Data ?? new List<ClienteResponseDTO>();
            var productos = productosResponse.Data ?? new List<ProductoResponseDTO>();

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

        private static void CalcularTotales(FacturaCreateViewModel facturaVM)
        {
            foreach (var detalle in facturaVM.Detalles)
            {
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                detalle.Impuesto = detalle.Subtotal * PorcentajeImpuesto;
                detalle.TotalLinea = detalle.Subtotal + detalle.Impuesto;
            }

            facturaVM.Subtotal = facturaVM.Detalles.Sum(detalle => detalle.Subtotal);
            facturaVM.Impuesto = facturaVM.Detalles.Sum(detalle => detalle.Impuesto);
            facturaVM.Total = facturaVM.Subtotal + facturaVM.Impuesto - facturaVM.Descuento;
        }
    }
}