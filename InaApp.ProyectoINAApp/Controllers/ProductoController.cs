using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using InaApp.ProyectoINAApp.Models.Producto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IMapper _mapper;
        private ProductoCreateViewModel model;

        public ProductoController(IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoService, IMapper mapper)
        {
            _productoService = productoService;
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
            catch (Exception ex) {
                ViewBag.Mensaje = "Ocurrió un error al obtener los productos.";
                return View(new List<ProductoIndexViewModel>());
            }
        }

        // GET: ProductoController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
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
        public ActionResult Create()
        {
            var productoVM = new ProductoCreateViewModel
            {
                CategoriaProductoId = 1
            };
            return View(productoVM);

        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductoCreateViewModel productoVM)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(productoVM);
                }

                productoVM.CategoriaProductoId = 1;

                //mapear el viewmodel a un dto para enviarlo al servicio
                var productoDTO = _mapper.Map<ProductoCreateDTO>(productoVM);

                //llamar al servicio para crear el producto
                var response = await _productoService.CrearAsync(productoDTO);

                if(!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(productoVM);
                }

                TempData["Mensaje"] = "Producto creado exitosamente.";


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Edit/5
        [HttpGet]
        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var response = await _productoService.ObtenerPorIdAsync(id);
                var productoVM = _mapper.Map<ProductoEditViewModel>(response.Data);

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
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(productoEditVM);
                }

                productoEditVM.CategoriaProductoId = 1;

                //mapear el viewmodel a un dto para enviarlo al servicio
                var productoDTO = _mapper.Map<ProductoUpdateDTO>(productoEditVM);

                //llamar al servicio para crear el producto
                var response = await _productoService.ActualizarAsync(productoDTO);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(productoEditVM);
                }

                TempData["Mensaje"] = "Producto modificado exitosamente.";


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
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
            catch
            {
                return View();
            }
        }
    }
}
