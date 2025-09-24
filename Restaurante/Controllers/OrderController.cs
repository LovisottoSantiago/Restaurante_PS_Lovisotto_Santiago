using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Application.Models;
using Application.Response;
using Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Examples.OrderExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/v1/Order
        [HttpPost]
        [ServiceFilter(typeof(OrderItemValidationFilter))]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Crear nueva orden", Description = "Crea una nueva orden con los platos solicitados por el cliente.")]
        [SwaggerRequestExample(typeof(OrderRequest), typeof(OrderRequestExample))]
        [SwaggerResponse(StatusCodes.Status201Created, "Orden creada exitosamente", typeof(OrderCreateReponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(OrderCreateResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de orden inválidos", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(OrderErrorExamples))]
        public async Task<IActionResult> Create([FromBody][Required] OrderRequest request)
        {
            var order = await _mediator.Send(new CreateOrderCommand(request));
            return CreatedAtAction(nameof(GetById), new { id = order.OrderNumber }, order);
        }

        // GET /api/v1/Order
        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Buscar órdenes",
            Description = "Obtiene una lista de órdenes con filtros opcionales.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de órdenes obtenida exitosamente", typeof(IEnumerable<OrderDetailsResponse>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OrderDetailsResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Parámetros de búsqueda inválidos", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(OrderSearchErrorExample))]
        public async Task<IActionResult> Get([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] int? status)
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery(from, to, status));
            return Ok(orders);
        }

        // GET /api/v1/Order/{id}
        [HttpGet("{id:long}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener orden por número", Description = "Obtiene los detalles completos de una orden específica.")]
        [ProducesResponseType(typeof(OrderDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            return Ok(order);
        }

        // PUT /api/v1/Order/{id}
        [HttpPut("{id:long}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Actualizar orden existente", Description = "Actualiza los items de una orden existente.")]
        [SwaggerRequestExample(typeof(OrderUpdateRequest), typeof(OrderUpdateRequestExample))]
        [SwaggerResponse(StatusCodes.Status200OK, "Orden actualizada exitosamente", typeof(OrderUpdateReponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(OrderUpdateResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de actualización inválidos", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(OrderUpdateErrorExamples))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Orden no encontrada", typeof(ApiError))]
        public async Task<IActionResult> Update(long id, [FromBody] OrderUpdateRequest request)
        {
            var response = await _mediator.Send(new UpdateOrderCommand(id, request));
            return Ok(response);
        }


        // PATCH /api/v1/Order/{id}/item/{itemId}
        [HttpPatch("{id:long}/item/{itemId:long}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Actualizar estado de item individual", Description = "Actualiza el estado de un item específico dentro de una orden.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Estado del item actualizado exitosamente", typeof(OrderUpdateReponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Estado inválido o transición no permitida", typeof(ApiError))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Orden o item no encontrado", typeof(ApiError))]
        public async Task<IActionResult> UpdateItem([FromRoute] long id, [FromRoute] long itemId, [FromBody] OrderItemUpdateRequest request)
        {
            var result = await _mediator.Send(new UpdateOrderItemCommand(id, itemId, request));
            return Ok(result);
        }


    }
}
