using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Cliente;
using inaApp.Entities;
using inaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/Cliente")]
    public class ClienteController : Controller
    {

        private readonly IGenericServices<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> _clienteService;

        public ClienteController(IGenericServices<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO> clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: ClienteController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {

                var lista = await _clienteService.ObtenerTodosAsync();

                if (lista == null || lista.Count == 0)
                {
                    return NotFound("No hay datos");
                }

                return Ok(lista);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, Contacte con el administrador");
            }
        }

        // GET: ClienteController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El id del cliente debe ser mayor a 0");
                }

                var cliente = await _clienteService.ObtenerPorIdAsync(id);

                return Ok(cliente);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, Contacte con el administrador");
            }
        }

        // POST: ClienteController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ClienteCreateDTO clienteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var nuevoCliente = await _clienteService.CrearAsync(clienteDTO);
                return Created("Cliente Creado", nuevoCliente);
            }
            catch (InvalidClientNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidClientIdentificationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateIdentificationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }

        // GET: ClienteController/Edit/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] ClienteUpdateDTO clienteDTO)
        {
            try
            {

                if (id <= 0)
                {
                    return BadRequest("El id del cliente debe ser mayor a 0");
                }

                if (clienteDTO == null)
                {
                    return BadRequest("Debe enviar la información del cliente");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                clienteDTO.IdCliente = id;

                var clienteActualizado = await _clienteService.ActualizarAsync(clienteDTO);

                return Ok(clienteActualizado);
            }
            catch (InvalidClientNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidClientIdentificationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateIdentificationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }

        // POST: ClienteController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("El id del cliente debe ser mayor a 0");
                }

                var result = await _clienteService.EliminarAsync(id);

                return result ? Ok("Cliente eliminado") : BadRequest("Cliente no encontrado");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, Contacte con el administrador");
            }
        }
    }
}
