using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroRepository _repository;

        public VaiTroController(IVaiTroRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VaiTro>>> GetAllVt()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<VaiTro>> GetVtById(int id)
        {
            try
            {
                var vaiTro = await _repository.GetByIdAsync(id);
                if (vaiTro == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }
                return Ok(vaiTro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddVaiTro([FromBody] VaiTro vaiTro)
        {
            if (vaiTro == null)
            {
                return BadRequest("Role data is null.");
            }

            try
            {
                await _repository.AddAsync(vaiTro);
                return CreatedAtAction(nameof(GetVtById), new { id = vaiTro.ID_Vai_Tro }, vaiTro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVaiTro(int id, [FromBody] VaiTro vaiTro)
        {
            if (vaiTro == null || vaiTro.ID_Vai_Tro != id)
            {
                return BadRequest("Role data is null or ID mismatch.");
            }

            try
            {
                await _repository.UpdateAsync(id, vaiTro);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVaiTro(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Role with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}