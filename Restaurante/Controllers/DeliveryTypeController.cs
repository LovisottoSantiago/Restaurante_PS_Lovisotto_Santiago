using Application.Features.DeliveryTypes.Queries;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Examples.DeliveryTypeExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeliveryTypeController : ControllerBase
    {        
        private readonly IMediator _mediator;

        public DeliveryTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/v1/DeliveryType
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener tipos de entrega", Description = "Obtiene todos los tipos de entrega disponibles para las órdenes.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de tipos de entrega obtenida exitosamente", typeof(IReadOnlyList<GenericResponse>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeliveryTypeResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            var deliveryTypes = await _mediator.Send(new GetAllDeliveryTypesQuery());
            return Ok(deliveryTypes); // 200
        }
    }
}
