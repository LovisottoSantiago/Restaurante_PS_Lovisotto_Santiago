using Application.Interfaces.Service;
using Application.Models;
using Application.Response;
using Microsoft.AspNetCore.Mvc;
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
        [SwaggerOperation(
            Summary = "Crear nuevo plato",
            Description = "Crea un nuevo plato en el menú del restaurante."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] DishRequest request)
        {
            try
            {
                var dish = await _service.CreateAsync(request);                
                return CreatedAtAction(nameof(GetById), new { id = dish.Id }, dish); // 201
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Buscar platos",
            Description = "Obtiene una lista de platos del menú con opciones de filtrado y ordenamiento."
        )]
        [ProducesResponseType(typeof(IEnumerable<DishResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] int? category, [FromQuery] SortDirection? sortByPrice, [FromQuery] bool onlyActive = true)
        {
            try
            {
                var dishes = await _service.GetAllAsync(name, category, sortByPrice, onlyActive);
                return Ok(dishes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dish = await _service.GetByIdAsync(id);
            if (dish == null) return NotFound(); // 404

            return Ok(dish); // 200
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar plato existente",
            Description = "Actualiza todos los campos de un plato existente en el menú."
        )]
        [ProducesResponseType(typeof(DishResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DishUpdateRequest request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request);
                return Ok(updated);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiError { Message = ex.Message });
            }
        }

    }
}
