using Application.Features.Statuses.Queries;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Examples.StatusExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StatusController : ControllerBase
    {        
        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/v1/Status
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener estados de órdenes", Description = "Obtiene todos los estados posibles para las órdenes y sus items.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de estados obtenida exitosamente", typeof(IReadOnlyList<GenericResponse>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(StatusResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _mediator.Send(new GetAllStatusesQuery());
            return Ok(statuses); // 200
        }
    }
}
