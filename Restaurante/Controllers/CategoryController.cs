using Application.Features.Categories.Queries;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Examples.CategoryExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Restaurante.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {        
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/v1/Category
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener categorías de platos", Description = "Obtiene todas las categorías disponibles para clasificar platos.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de categorías obtenida exitosamente", typeof(IReadOnlyList<CategoryResponse>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(categories); // 200
        }
    }
}
