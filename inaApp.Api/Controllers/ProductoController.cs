using inaApp.Common.Exceptions;
using inaApp.Common.Interfaces;
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

        private readonly IGenericServices<Producto> _productoService;

        public ProductoController(IGenericServices<Producto> productoService)
        {
            _productoService = productoService;
        }


        // GET: ProductoController
        [HttpGet]
        public async Task<ActionResult> Index()
        {

            try
            {
                var lista = await _productoService.ObtenerTodosAsync();

                if (lista ==null || lista.Count == 0)
                {
                    return NotFound("No hay datos");
                }

                return Ok(lista);


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
                var producto = await _productoService.ObtenerPorIdAsync(id);

                return Ok(producto);
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
        public async Task<ActionResult> Create([FromBody] Producto producto)
        {
            try
            {
                var nuevoProducto = await _productoService.CrearAsync(producto);
                return Created("Producto Creado", nuevoProducto);
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
        public async Task<ActionResult> Edit(int id, [FromBody] Producto producto)
        {
            //editar un producto
            try
            {
                producto.estado = true; // Asegura que el producto esté activo al editarlo

                var productoActualizado = await _productoService.ActualizarAsync(producto);

                return Ok(productoActualizado);
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

                var result = await _productoService.EliminarAsync(id);

                return result ? Ok("Producto eliminado") : BadRequest("Producto no encontrado");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error, Contacte con el administrador");
            }
        }
    }
}
