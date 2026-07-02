using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.CategoriaProducto;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using InaApp.ProyectoINAApp.Models.Producto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public ProductoController(
            IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoService,
            IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO> categoriaService,
            IMapper mapper)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        // GET: ProductoController
        public async Task<ActionResult> Index()
        {
            try
            {
                //requiero ir a todos los productos y mostrarlos en la vista
                var lista = await _productoService.ObtenerTodosAsync();

                //mapeo la lista de productos a una lista de viewmodels para pasarlos a la vista
                var listaViewModel = _mapper.Map<List<ProductoIndexViewModel>>(lista.Data);
                //pasamos la lista de viewmodels a la vista por el modelo, para que la vista pueda iterar sobre ellos y mostrarlos
                return View(listaViewModel);
            }
            catch (NotFoundException)
            {
                //model: pasar datos, dtos, entities, viewmodels, etc
                //viewbag: pasar datos simples, string, int, bool, etc
                //viewdata: pasar datos simples, string, int, bool, etc
                ViewBag.Mensaje = "No se encontraron productos.";
                return View(new List<ProductoIndexViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = "Ocurrió un error al obtener los productos.";
                return View(new List<ProductoIndexViewModel>());
            }
        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _productoService.ObtenerPorIdAsync(id);
                var productoVM = _mapper.Map<ProductoIndexViewModel>(response.Data);

                return View(productoVM);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: ProductoController/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
            var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
            var productoVM = new ProductoCreateViewModel
            {
                Categorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToList()
            };

            return View(productoVM);
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductoCreateViewModel productoVM)
        {
            if (!ModelState.IsValid)
            {
                var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                productoVM.Categorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = c.Id == productoVM.CategoriaProductoId
                }).ToList();

                return View(productoVM);
            }

            try
            {
                var productoDTO = _mapper.Map<ProductoCreateDTO>(productoVM);
                var response = await _productoService.CrearAsync(productoDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                    var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                    productoVM.Categorias = categorias.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre,
                        Selected = c.Id == productoVM.CategoriaProductoId
                    }).ToList();

                    return View(productoVM);
                }

                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                productoVM.Categorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = c.Id == productoVM.CategoriaProductoId
                }).ToList();

                return View(productoVM);
            }

            }

        // GET: ProductoController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var response = await _productoService.ObtenerPorIdAsync(id);
                var productoVM = _mapper.Map<ProductoEditViewModel>(response.Data);
                var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                productoVM.Categorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = c.Id == productoVM.CategoriaProductoId
                }).ToList();

                return View(productoVM);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductoEditViewModel productoEditVM)
        {
            if (!ModelState.IsValid)
            {
                var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                productoEditVM.Categorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = c.Id == productoEditVM.CategoriaProductoId
                }).ToList();

                return View(productoEditVM);
            }

            try
            {
                var productoDTO = _mapper.Map<ProductoUpdateDTO>(productoEditVM);
                var response = await _productoService.ActualizarAsync(productoDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                    var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                    productoEditVM.Categorias = categorias.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre,
                        Selected = c.Id == productoEditVM.CategoriaProductoId
                    }).ToList();

                    return View(productoEditVM);
                }

                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var categoriasResponse = await _categoriaService.ObtenerTodosAsync();
                var categorias = categoriasResponse.Data ?? new List<CategoriaProductoResponseDTO>();
                productoEditVM.Categorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre,
                    Selected = c.Id == productoEditVM.CategoriaProductoId
                }).ToList();

                return View(productoEditVM);
            }
        }

        // GET: ProductoController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _productoService.ObtenerPorIdAsync(id);
            if (!response.Success)
            {
                TempData["Mensaje"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            var productoVM = _mapper.Map<ProductoIndexViewModel>(response.Data);
            return View(productoVM);
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _productoService.EliminarAsync(id);
                if (!response.Success)
                {
                    TempData["Mensaje"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
                TempData["Mensaje"] = "Producto eliminado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
