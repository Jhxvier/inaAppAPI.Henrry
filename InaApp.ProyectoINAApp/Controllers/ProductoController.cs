using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ProductoController : Controller
    {

        private readonly IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;
        private readonly IMapper _mapper;

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
                return View();
            }
            catch (Exception ex) {
                ViewBag.Mensaje = "Ocurrió un error al obtener los productos.";
                return View();
            }
        }

        // GET: ProductoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
