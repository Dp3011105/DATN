using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly ITaiKhoanRepository _repository;

        public TaiKhoanController(ITaiKhoanRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTk()
        {
            try
            {
                var taiKhoans = await _repository.GetAllAsync();
                return Ok(taiKhoans);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var taiKhoan = await _repository.GetByIdAsync(id);
                if (taiKhoan == null)
                {
                    return NotFound("TaiKhoan not found.");
                }
                return Ok(taiKhoan);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateTaiKhoan([FromBody] TaiKhoan taiKhoan)
        {
            if (taiKhoan == null)
            {
                return BadRequest("TaiKhoan is null.");
            }
            if (string.IsNullOrEmpty(taiKhoan.Ten_Nguoi_Dung) || string.IsNullOrEmpty(taiKhoan.Mat_Khau))
            {
                return BadRequest("Ten_Nguoi_Dung and Mat_Khau are required.");
            }
            try
            {
                await _repository.AddAsync(taiKhoan);
                return CreatedAtAction(nameof(GetById), new { id = taiKhoan.ID_Tai_Khoan }, taiKhoan);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTaiKhoan(int id, [FromBody] TaiKhoan taiKhoan)
        {
            if (taiKhoan == null || taiKhoan.ID_Tai_Khoan != id)
            {
                return BadRequest("TaiKhoan is null or ID mismatch.");
            }
            try
            {
                await _repository.UpdateAsync(id, taiKhoan);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("TaiKhoan not found.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaiKhoan(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("TaiKhoan not found.");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}