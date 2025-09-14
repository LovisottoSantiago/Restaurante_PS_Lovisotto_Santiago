using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(OrderItemValidationFilter))]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        // POST /api/v1/Order
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Crear nueva orden", Description = "Crea una nueva orden con los platos solicitados por el cliente.")]
        [ProducesResponseType(typeof(OrderCreateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            var order = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = order.OrderNumber }, order);
        }

        // GET /api/v1/Order
        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener todas las órdenes",
            Description = "Devuelve la lista de órdenes con filtros opcionales por fecha y estado.")]
        [ProducesResponseType(typeof(IEnumerable<OrderDetailsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? status)
        {
            var orders = await _service.GetAllAsync(from, to, status);
            return Ok(orders);
        }

        // GET /api/v1/Order/{id}
        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener orden por ID",
            Description = "Devuelve el detalle de una orden específica por su número.")]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var order = await _service.GetByIdAsync(id);
            return Ok(order);
        }
    }
}
