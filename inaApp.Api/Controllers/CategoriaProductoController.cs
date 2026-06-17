using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.CategoriaProducto;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/CategoriaProducto")]
    public class CategoriaProductoController : Controller
    {
        private readonly IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO> _categoriaService;

        public CategoriaProductoController(IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO> categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var response = await _categoriaService.ObtenerTodosAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _categoriaService.ObtenerPorIdAsync(id);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoriaProductoCreateDTO categoriaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _categoriaService.CrearAsync(categoriaDTO);
                return Created("Categoria Creada", response);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] CategoriaProductoUpdateDTO categoriaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                categoriaDTO.Id = id;
                var response = await _categoriaService.ActualizarAsync(categoriaDTO);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id no puede ser nulo");
                }

                var response = await _categoriaService.EliminarAsync(id);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
