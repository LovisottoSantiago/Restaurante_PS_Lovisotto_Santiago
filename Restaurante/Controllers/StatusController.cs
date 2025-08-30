using Application.Interfaces;
using Application.Models.Status;
using Microsoft.AspNetCore.Mvc;

namespace Restaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _service;

        public StatusController(IStatusService service)
        {
            _service = service;
        }

        // GET: api/status
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result); // 200
        }

        // GET: api/status/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound(); // 404

            return Ok(result); // 200
        }

        // POST: api/status
        [HttpPost]
        public async Task<IActionResult> Create(CreateStatusRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); // 201
        }

        // PUT: api/status/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateStatusRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result); // 200
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteSuccess = await _service.DeleteAsync(id);
            if (!deleteSuccess) return NotFound(); // 404

            return NoContent(); // 204
        }
    }
}
