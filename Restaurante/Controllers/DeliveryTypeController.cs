using Application.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeliveryTypeController : ControllerBase
    {
        private IDeliveryTypeService _service;

        public DeliveryTypeController(IDeliveryTypeService service)
        {
            _service = service;
        }

        // GET /api/v1/DeliveryType
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener tipos de entrega", Description = "Obtiene todos los tipos de entrega disponibles para las órdenes.")]
        public async Task<IActionResult> GetAll()
        {
            var deliveryTypes = await _service.GetAllAsync();
            return Ok(deliveryTypes); // 200
        }
    }
}
