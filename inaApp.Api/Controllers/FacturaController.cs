using inaApp.Common.Interfaces;
using inaApp.DTOs.Factura;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace inaApp.Api.Controllers
{
    [ApiController]
    [Route("api/facturas")]
    public class FacturaController : Controller
    {
        private readonly IFacturaService<
            FacturaResponseDTO,
            FacturaListDTO,
            FacturaCreateDTO> _facturaService;

        public FacturaController(IFacturaService<
                FacturaResponseDTO,
                FacturaListDTO,
                FacturaCreateDTO> facturaService)
        {
            _facturaService = facturaService;
        }

        // GET: FacturaController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var response = await _facturaService.ObtenerTodosAsync();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }

        // GET: FacturaController/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var response = await _facturaService.ObtenerPorIdAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }

        // GET: FacturaController/Create
        /*[HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }*/

        // POST: FacturaController/Create
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] FacturaCreateDTO facturaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (facturaDTO.Detalles == null || facturaDTO.Detalles.Count == 0)
                {
                    return BadRequest("Debe agregar al menos un producto.");
                }

                var response = await _facturaService.CrearAsync(facturaDTO);

                if (!response.Success || response.Data == null)
                    return BadRequest(response.Message);

                return Created("Factura creada", response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }


        // GET: FacturaController/Edit/5
        /*[HttpGet("Edit/{id}")]
        public ActionResult Edit(int id)
        {
            return View();
        }*/

        // POST: FacturaController/Edit/5
        /*[HttpPost("Edit/{id}")]
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
        }*/

        // GET: FacturaController/Delete/5
        /*[HttpGet("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            return View();
        }*/

        // POST: FacturaController/Delete/5
        /*[HttpPost("Delete/{id}")]
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
        }*/

        /*[HttpPut("{id}/anular")]
        public async Task<ActionResult> Anular(int id)
        {
            try
            {
                var response = await _facturaService.AnularAsync(id);
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error, contacte con el administrador");
            }
        }*/
    }
}
