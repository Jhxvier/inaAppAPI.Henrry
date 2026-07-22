using AutoMapper;
using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using InaApp.ProyectoINAApp.Models.Categoria;
using InaApp.ProyectoINAApp.Models.Cliente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InaApp.ProyectoINAApp.Controllers
{
    public class ClienteController : Controller
    {

        private readonly IGenericServices<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IGenericServices<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        // GET: ClienteController
        public async Task<ActionResult> Index()
        {
            try
            {
                var response = await _clienteService.ObtenerTodosAsync();
                return View(_mapper.Map<List<ClienteIndexViewModel>>(response.Data));
            }
            catch (NotFoundException)
            {
                ViewBag.Mensaje = "No se encontraron clientes.";
                return View(new List<ClienteIndexViewModel>());
            }

        }

        // GET: ClienteController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdAsync(id);
                return View(_mapper.Map<ClienteIndexViewModel>(response.Data));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        // GET: ClienteController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ClienteCreateViewModel());
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClienteCreateViewModel clienteVM)
        {
            if (!ModelState.IsValid) return View(clienteVM);

            try
            {
                var response = await _clienteService.CrearAsync(_mapper.Map<ClienteCreateDTO>(clienteVM));
                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(clienteVM);
                }

                TempData["Mensaje"] = "Cliente creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(clienteVM);
            }

        }

        // GET: ClienteController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdAsync(id);
                return View(_mapper.Map<ClienteEditViewModel>(response.Data));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ClienteEditViewModel clienteVM)
        {
            if (!ModelState.IsValid) return View(clienteVM);

            try
            {
                var response = await _clienteService.ActualizarAsync(_mapper.Map<ClienteUpdateDTO>(clienteVM));
                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(clienteVM);
                }

                TempData["Mensaje"] = "Cliente actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(clienteVM);
            }

        }

        // GET: ClienteController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _clienteService.ObtenerPorIdAsync(id);
                return View(_mapper.Map<ClienteIndexViewModel>(response.Data));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _clienteService.EliminarAsync(id);
                TempData["Mensaje"] = response.Success ? "Cliente eliminado correctamente." : response.Message;
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
