using BE.DTOs;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangRepository _repository;

        public KhachHangController(IKhachHangRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKh()
        {
            try
            {
                var customers = await _repository.GetAllKhachHang();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKh(int id)
        {
            try
            {
                var customer = await _repository.GetKhachHangById(id);
                return Ok(customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddKh([FromBody] KhachHangDTO entity)
        {
            if (entity == null)
            {
                return BadRequest("Customer data is invalid.");
            }
            try
            {
                await _repository.AddKhachHang(entity);
                return CreatedAtAction(nameof(GetKh), new { id = entity.ID_Khach_Hang }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKh(int id, [FromBody] KhachHangDTO entity)
        {
            try
            {
                await _repository.UpdateKhachHang(id, entity);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKh(int id)
        {
            try
            {
                await _repository.DeleteKhachHang(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}