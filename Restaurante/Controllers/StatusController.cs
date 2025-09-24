using Application.Features.Statuses.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _mediator.Send(new GetAllStatusesQuery());
            return Ok(statuses); // 200
        }
    }
}
