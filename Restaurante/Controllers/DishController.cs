using Application.Exceptions;
using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
using Restaurante.Examples;
using Swashbuckle.AspNetCore.Annotations;

namespace Infrastructure.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService dishService)
        {
            _service = dishService;
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Crear nuevo plato", Description = "Crea un nuevo plato en el menú del restaurante.")]
        [SwaggerResponse(StatusCodes.Status201Created, "Plato creado exitosamente", typeof(DishResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos", typeof(ApiErrorExamples))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un plato con el mismo nombre", typeof(ApiErrorConflictExample))]
        public async Task<IActionResult> Create([FromBody] DishRequest request)
        {
            try
            {
                var dish = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish); // 201
            }
            catch (CustomException ex)
            {
                return BadRequest(new ApiError { Message = string.Join(" | ", ex.Errors) });  // 400
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiError { Message = ex.Message }); // 409
            }
        }


        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Buscar platos", Description = "Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de platos obtenida exitosamente", typeof(IEnumerable<DishResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Parámetros de búsqueda inválidos", typeof(ApiErrorExamples))]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] int? category, [FromQuery] SortDirection? sortByPrice, [FromQuery] bool onlyActive = true)
        {
            try
            {
                var dishes = await _service.GetAllAsync(name, category, sortByPrice, onlyActive);
                return Ok(dishes);
            }
            catch (CustomException ex)
            {
                return BadRequest(new ApiError { Message = string.Join(" | ", ex.Errors) }); // 400
            }
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _service.GetByIdAsync(id);
            if (dish == null) return NotFound(new ApiError { Message = "Plato no encontrado" }); // 404

            return Ok(dish); // 200
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Actualizar plato existente", Description = "Actualiza todos los campos de un plato existente en el menú.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plato actualizado exitosamente", typeof(DishResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Datos de entrada inválidos", typeof(ApiErrorExamples))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Plato no encontrado", typeof(ApiErrorNotFoundExample))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflicto - nombre duplicado", typeof(ApiErrorConflictExample))]
        public async Task<IActionResult> Update(Guid id, [FromBody] DishUpdateRequest request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request);
                return Ok(updated);
            }
            catch (CustomException ex)
            {
                // Si entre los errores estaba "Plato no encontrado", devolver 404
                if (ex.Errors.Any(e => e.Contains("Plato no encontrado", StringComparison.OrdinalIgnoreCase)))
                    return NotFound(new ApiError { Message = string.Join(" | ", ex.Errors) }); // 404

                return BadRequest(new ApiError { Message = string.Join(" | ", ex.Errors) }); // 400
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiError { Message = ex.Message }); // 409
            }
        }
    }
}
