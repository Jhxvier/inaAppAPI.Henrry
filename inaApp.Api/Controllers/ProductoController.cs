using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/Producto")]
    public class ProductoController : Controller
    {

        private readonly IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> _productoService;

        public ProductoController(IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO> productoService)
        {
            _productoService = productoService;
        }


        // GET: ProductoController
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            try
            {
                var response = await _productoService.ObtenerTodosAsync();

                return Ok(response);


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

        // GET: ProductoController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            //obtener el producto por id utilizando el método ObtenerPorIdAsync
            try
            {
                var response = await _productoService.ObtenerPorIdAsync(id);

                return Ok(response);
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

        // POST: ProductoController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductoCreateDTO productoDTO)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _productoService.CrearAsync(productoDTO);

                return Created("Producto Creado", response);
            }
            catch (InvalidPriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }


        }

        // GET: ProductoController/Edit/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, [FromBody] ProductoUpdateDTO productoDTO)
        {
            //editar un producto
            try
            {
                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                productoDTO.Id = id;

                var response = await _productoService.ActualizarAsync(productoDTO);

                return Ok(response);
            }
            catch(InvalidPriceException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(InvalidStockException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }


        // POST: ProductoController/Delete/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {

            try
            {

                if (id <= 0)
                {
                    return BadRequest("Id no puede ser nulo");
                }

                var response = await _productoService.EliminarAsync(id);

                return response.Data ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, Contacte con el administrador");
            }
        }
    }
}
