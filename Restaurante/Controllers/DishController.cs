using Application.Interfaces;
using Application.Models;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Restaurante.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        // POST: /api/v1/Dish
        [HttpPost]
        [SwaggerOperation(
            Summary = "Crear nuevo plato",
            Description = @"Crea un nuevo plato en el menú del restaurante.

            **Validaciones:**
            - El nombre del plato debe ser único
            - El precio debe ser mayor a 0
            - La categoría debe existir
            ")]
        [SwaggerResponse(201, "Plato creado exitosamente", typeof(DishResponse))]
        [SwaggerResponse(400, "Datos de entrada inválidos", typeof(ApiError))]
        [SwaggerResponse(409, "Ya existe un plato con el mismo nombre", typeof(ApiError))]
        public async Task<IActionResult> Create([FromBody] DishRequest request)
        {
            if (request.Price <= 0 || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new ApiError { Message = "Datos inválidos" });

            try
            {
                var dish = await _dishService.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish);
            }
            catch (InvalidOperationException)
            {
                return Conflict(new ApiError { Message = "Ya existe un plato con ese nombre" });
            }
        }

        // GET: /api/v1/Dish
        [HttpGet]
        [SwaggerOperation(
            Summary = "Buscar platos",
            Description = @"Obtiene una lista de platos con filtros y ordenamiento.

            **Filtros:**
            - nombre (parcial)
            - categoría
            - activos/todos

            **Ordenamiento:**
            - precio asc/desc
            ")]
        [SwaggerResponse(200, "Lista de platos obtenida exitosamente", typeof(IEnumerable<DishResponse>))]
        [SwaggerResponse(400, "Parámetros de búsqueda inválidos", typeof(ApiError))]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] int? categoryId,
            [FromQuery] string? sortDirection,
            [FromQuery] bool onlyActive = true)
        {
            if (!string.IsNullOrEmpty(sortDirection) &&
                sortDirection.ToUpper() != "ASC" &&
                sortDirection.ToUpper() != "DESC")
            {
                return BadRequest(new ApiError { Message = "Parámetros de ordenamiento inválidos" });
            }

            var dishes = await _dishService.GetAllAsync(name, categoryId, sortDirection);
            if (onlyActive)
                dishes = dishes.Where(d => d.IsActive).ToList();

            return Ok(dishes);
        }

        // GET: /api/v1/Dish/{id}
        [HttpGet("{id:guid}")]
        [ApiExplorerSettings(IgnoreApi = true)] // Solo se usa internamente para CreatedAtAction
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
                return NotFound(new ApiError { Message = "Plato no encontrado" });

            return Ok(dish);
        }

        // PUT: /api/v1/Dish/{id}
        [HttpPut("{id:guid}")]
        [SwaggerOperation(
            Summary = "Actualizar plato existente",
            Description = @"Actualiza todos los campos de un plato existente en el menú.

            **Validaciones:**
            - El plato debe existir
            - El nombre debe ser único
            - El precio debe ser mayor a 0
            - La categoría debe existir
            ")]
        [SwaggerResponse(200, "Plato actualizado exitosamente", typeof(DishResponse))]
        [SwaggerResponse(400, "Datos de entrada inválidos", typeof(ApiError))]
        [SwaggerResponse(404, "Plato no encontrado", typeof(ApiError))]
        [SwaggerResponse(409, "Conflicto - nombre duplicado", typeof(ApiError))]
        public async Task<IActionResult> Update(Guid id, [FromBody] DishUpdateRequest request)
        {
            if (request.Price <= 0)
                return BadRequest(new ApiError { Message = "El precio debe ser mayor a cero" });

            try
            {
                var dish = await _dishService.UpdateAsync(id, request);
                return Ok(dish);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiError { Message = "Plato no encontrado" });
            }
            catch (InvalidOperationException)
            {
                return Conflict(new ApiError { Message = "Ya existe un plato con ese nombre" });
            }
        }
    }
}
