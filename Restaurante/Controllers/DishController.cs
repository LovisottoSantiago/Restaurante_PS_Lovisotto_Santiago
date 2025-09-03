using Application.Interfaces;
using Application.Models;
using Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;
        public DishController(IDishService service) => _service = service;

        // POST /api/v1/Dish
        [HttpPost]
        public async Task<ActionResult<DishResponse>> Create([FromBody] DishRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new ApiError { Message = "El nombre del plato es obligatorio" });
            if (request.Price <= 0)
                return BadRequest(new ApiError { Message = "El precio debe ser mayor a cero" });

            try
            {
                var created = await _service.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, created);
            }
            catch (InvalidOperationException ex) 
            {
                return Conflict(new ApiError { Message = ex.Message }); 
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        // GET /api/v1/Dish?name=&category=&sortByPrice=&onlyActive=
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishResponse>>> Get(
            [FromQuery] string? name,
            [FromQuery(Name = "category")] int? category,
            [FromQuery] string? sortByPrice,
            [FromQuery] bool onlyActive = true)
        {
            if (!string.IsNullOrWhiteSpace(sortByPrice))
            {
                var v = sortByPrice.ToLowerInvariant();
                if (v != "asc" && v != "desc")
                    return BadRequest(new ApiError { Message = "Parámetros de ordenamiento inválidos" });
            }

            try
            {
                var result = await _service.GetAllAsync(name, category, sortByPrice, onlyActive);
                return Ok(result);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }

        // PUT /api/v1/Dish/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DishResponse>> Update([FromRoute] Guid id, [FromBody] DishUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new ApiError { Message = "El nombre del plato es obligatorio" });
            if (request.Price <= 0)
                return BadRequest(new ApiError { Message = "El precio debe ser mayor a cero" });

            try
            {
                var updated = await _service.UpdateAsync(id, request);
                return Ok(updated);
            }
            catch (KeyNotFoundException) // not found
            {
                return NotFound(new ApiError { Message = "Plato no encontrado" });
            }
            catch (InvalidOperationException ex) // duplicado
            {
                return Conflict(new ApiError { Message = ex.Message }); // "Ya existe un plato con ese nombre"
            }
            catch (ArgumentException ex) // categoría/validación
            {
                return BadRequest(new ApiError { Message = ex.Message });
            }
        }
    }
}
