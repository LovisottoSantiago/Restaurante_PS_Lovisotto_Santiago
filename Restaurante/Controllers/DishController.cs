using Application.Features.Dishes.Commands;
using Application.Features.Dishes.Queries;
using Application.Models;
using Application.Response;
using Application.UseCases.DishUseCases.UpdateDish;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Examples.DishExamples;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/v1/Dish
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Crear nuevo plato", Description = "Crea un nuevo plato en el menú del restaurante.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Plato creado exitosamente", typeof(DishResponse))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(DishResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ApiErrorPostBadRequestExample))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un plato con el mismo nombre", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ApiErrorConflictExample))]
        [SwaggerRequestExample(typeof(DishRequest), typeof(DishRequestExample))]
        public async Task<IActionResult> Create([FromBody] DishRequest request)
        {            
            var dish = await _mediator.Send(new CreateDishCommand(request));
            return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish); // 201
        }

        // GET /api/v1/Dish
        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Buscar platos", Description = "Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de platos obtenida exitosamente", typeof(IEnumerable<DishResponse>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DishListResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Parámetros de búsqueda inválidos", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ApiErrorGetBadRequestExample))]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] int? category, [FromQuery] SortDirection? sortByPrice, [FromQuery] bool onlyActive = true)
        {            
            var dishes = await _mediator.Send(new GetAllDishesQuery(name, category, sortByPrice, onlyActive));
            return Ok(dishes); // 200
        }

        // GET /api/v1/Dish/{id}
        [HttpGet("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Obtener plato por ID", Description = "Obtiene los detalles completos de un plato específico.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plato encontrado exitosamente", typeof(DishResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DishResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Formato de ID inválido", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ApiErrorGetByIdBadRequestExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Plato no encontrado", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ApiErrorNotFoundExample))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _mediator.Send(new GetDishByIdQuery(id));
            return Ok(dish); 
        }

        // PUT /api/v1/Dish/{id}
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Actualizar plato existente", Description = "Actualiza todos los campos de un plato existente en el menú.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plato actualizado exitosamente", typeof(DishResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DishResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ApiErrorPutBadRequestExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Plato no encontrado", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ApiErrorNotFoundExample))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto - nombre duplicado", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ApiErrorConflictExample))]
        [SwaggerRequestExample(typeof(DishUpdateRequest), typeof(DishUpdateRequestExample))]
        public async Task<IActionResult> Update(Guid id, [FromBody] DishUpdateRequest request)
        {
            var updated = await _mediator.Send(new UpdateDishCommand(id, request));
            return Ok(updated); // 200
        }

        // DELETE api/v1/dish/{id}
        [HttpDelete("{id:guid}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Eliminar plato", Description = "Elimina un plato del menú del restaurante.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plato eliminado exitosamente", typeof(DishResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DishResponseExample))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Plato no encontrado", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ApiErrorNotFoundExample))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "No se puede eliminar - plato en uso", typeof(ApiError))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ApiErrorDeleteConflictExample))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dish = await _mediator.Send(new DeleteDishCommand(id));
            return Ok(dish);
        }
    }
}
