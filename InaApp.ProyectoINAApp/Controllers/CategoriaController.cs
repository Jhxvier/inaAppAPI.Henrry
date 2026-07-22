using AutoMapper;
using inaApp.Common.Interfaces;
using inaApp.DTOs.CategoriaProducto;
using inaApp.Entities;
using InaApp.ProyectoINAApp.Models.Categoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class CategoriaController : Controller
    {

        private readonly IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO> _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO> categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        // GET: CategoriaController
        public async Task<ActionResult> Index()
        {
            var response = await _categoriaService.ObtenerTodosAsync();
            var categorias = _mapper.Map<List<CategoriaIndexViewModel>>(response.Data);
            return View(categorias);

        }

        // GET: CategoriaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _categoriaService.ObtenerPorIdAsync(id);
                return View(_mapper.Map<CategoriaIndexViewModel>(response.Data));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: CategoriaController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CategoriaCreateViewModel());
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoriaCreateViewModel categoriaVM)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaVM);
            }

            try
            {
                var dto = _mapper.Map<CategoriaProductoCreateDTO>(categoriaVM);
                var response = await _categoriaService.CrearAsync(dto);
                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(categoriaVM);
                }

                TempData["Mensaje"] = "Categoría creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoriaVM);
            }

        }

        // GET: CategoriaController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var response = await _categoriaService.ObtenerPorIdAsync(id);
                return View(_mapper.Map<CategoriaEditViewModel>(response.Data));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoriaEditViewModel categoriaVM)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaVM);
            }

            try
            {
                var dto = _mapper.Map<CategoriaProductoUpdateDTO>(categoriaVM);
                var response = await _categoriaService.ActualizarAsync(dto);
                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(categoriaVM);
                }

                TempData["Mensaje"] = "Categoría actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoriaVM);
            }

        }

        // GET: CategoriaController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _categoriaService.ObtenerPorIdAsync(id);
                return View(_mapper.Map<CategoriaIndexViewModel>(response.Data));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
;
        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _categoriaService.EliminarAsync(id);
                TempData["Mensaje"] = response.Success ? "Categoría eliminada correctamente." : response.Message;
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
