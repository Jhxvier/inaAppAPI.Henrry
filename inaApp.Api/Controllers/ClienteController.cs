using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/Cliente")]
    public class ClienteController : Controller
    {

        private readonly IGenericServices<Cliente> _clienteService;

        public ClienteController(IGenericServices<Cliente> clienteService)
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
        public async Task<ActionResult> Create([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest("El cliente es requerido");
                }

                cliente.Estado = true;
                var nuevoCliente = await _clienteService.CrearAsync(cliente);
                return Created("Cliente Creado", nuevoCliente);
            }
            catch (InvalidClientNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidClientBirthDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
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
        public async Task<ActionResult> Edit(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest("El cliente es requerido");
                }

                cliente.Id = id;
                cliente.Estado = true;

                var clienteActualizado = await _clienteService.ActualizarAsync(cliente);

                return Ok(clienteActualizado);
            }
            catch (InvalidClientNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidClientBirthDateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
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
                    return BadRequest("Id no puede ser nulo");
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