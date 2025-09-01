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

        /// <summary>
        /// Crear un nuevo plato
        /// </summary>
        /// <remarks>Endpoint para registrar un nuevo plato en el sistema.</remarks>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Plato creado correctamente", typeof(DishResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un plato con el mismo nombre", typeof(object))]
        public async Task<IActionResult> CreateDish([FromBody] CreateDishRequest createDishRequest)
        {
            try
            {
                var createdDish = await _dishService.CreateAsync(createDishRequest);
                return CreatedAtAction(nameof(GetDishById), new { id = createdDish.DishId }, createdDish);
            }
            catch (InvalidOperationException)
            {
                return Conflict(new ApiError { Message = "Ya existe un plato con ese nombre" });
            }
        }

        /// <summary>
        /// Buscar platos
        /// </summary>
        /// <remarks>Permite filtrar por nombre, categoría y ordenar por precio (ASC o DESC).</remarks>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Lista de platos obtenida correctamente", typeof(IEnumerable<DishResponse>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "El parámetro 'order' no es válido", typeof(object))]
        public async Task<IActionResult> GetDishes([FromQuery] string? name, [FromQuery] int? categoryId, [FromQuery] string? order)
        {
            if (!string.IsNullOrEmpty(order) && order.ToUpper() != "ASC" && order.ToUpper() != "DESC")
            {
                return BadRequest(new ApiError { Message = "El parámetro 'order' solo puede ser 'ASC' o 'DESC'." });
            }

            var dishes = await _dishService.GetAllAsync(name, categoryId, order);
            return Ok(dishes);
        }

        /// <summary>
        /// Obtener plato por ID
        /// </summary>
        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDishById(Guid id)
        {
            var dish = await _dishService.GetByIdAsync(id);
            if (dish == null)
            {
                return NotFound(new ApiError { Message = "Plato no encontrado" });
            }

            return Ok(dish);
        }

        /// <summary>
        /// Actualizar plato existente
        /// </summary>
        /// <remarks>Modifica los datos de un plato identificado por su ID.</remarks>
        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Plato actualizado correctamente", typeof(DishResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Plato no encontrado", typeof(object))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Ya existe un plato con el mismo nombre", typeof(object))]
        public async Task<IActionResult> UpdateDish(Guid id, [FromBody] UpdateDishRequest updateDishRequest)
        {
            try
            {
                var updatedDish = await _dishService.UpdateAsync(id, updateDishRequest);
                return Ok(updatedDish);
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
