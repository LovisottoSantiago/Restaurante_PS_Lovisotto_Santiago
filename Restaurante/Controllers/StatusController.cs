using Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StatusController : ControllerBase
    {
        private IStatusService _service;

        public StatusController(IStatusService service)
        {
            _service = service;
        }

        // GET /api/v1/Status
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener estados de órdenes", Description = "Obtiene todos los estados posibles para las órdenes y sus items.")]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _service.GetAllAsync();
            return Ok(statuses); // 200
        }
    }
}
