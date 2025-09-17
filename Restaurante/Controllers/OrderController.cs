using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
        [SwaggerOperation(Summary = "Obtener orden por ID", Description = "Devuelve el detalle de una orden específica por su número.")]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var order = await _service.GetByIdAsync(id);
            return Ok(order);
        }

        // PUT /api/v1/Order/{id}
        [HttpPut("{id:long}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Actualizar orden existente", Description = "Actualiza los items de una orden existente.")]
        [SwaggerRequestExample(typeof(OrderUpdateRequest), typeof(Restaurante.Examples.OrderExamples.OrderUpdateRequestExample))]
        [ProducesResponseType(typeof(OrderUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(long id, [FromBody] OrderUpdateRequest request)
        {
            var response = await _service.UpdateAsync(id, request);
            return Ok(response);
        }


        // PATCH /api/v1/Order/{id}/item/{itemId}
        [HttpPatch("{id:long}/item/{itemId:long}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Actualizar estado de item individual", Description = "Actualiza el estado de un item específico dentro de una orden.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del item actualizado exitosamente", typeof(OrderUpdateResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Estado inválido o transición no permitida", typeof(ApiError))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Orden o item no encontrado", typeof(ApiError))]
        public async Task<IActionResult> UpdateItem([FromRoute] long id, [FromRoute] long itemId, [FromBody] OrderItemUpdateRequest request)
        {
            var result = await _service.UpdateItemAsync(id, itemId, request);
            return Ok(result);
        }


    }
}
