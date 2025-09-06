using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repository;
        public HoaDonController(IHoaDonRepository repository) => _repository = repository;

        // GET api/HoaDon  -> trả DTO (màn danh sách)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDonDTO>>> GetAll()
            => Ok(await _repository.GetAllAsync());

        // GET api/HoaDon/entities -> trả entity (giữ tương thích code FE cũ)
        [HttpGet("entities")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetAllEntities()
            => Ok(await _repository.GetAllEntitiesAsync());

        // GET api/HoaDon/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoaDon = await _repository.GetByIdAsync(id);
            if (hoaDon == null) return NotFound();
            return Ok(hoaDon);
        }

        // POST api/HoaDon
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null) return BadRequest("HoaDon cannot be null.");
            await _repository.AddAsync(hoaDon);
            return CreatedAtAction(nameof(GetById), new { id = hoaDon.ID_Hoa_Don }, hoaDon);
        }

        // PUT api/HoaDon/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null || id != hoaDon.ID_Hoa_Don)
                return BadRequest("Invalid HoaDon data.");

            await _repository.UpdateAsync(id, hoaDon);
            return NoContent();
        }

        // DELETE api/HoaDon/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        // -------- PATCH trạng thái --------
        public class UpdateTrangThaiRequest
        {
            public string TrangThai { get; set; } = default!;
            public string? LyDoHuy { get; set; }
        }

        // PATCH api/HoaDon/5/TrangThai
        [HttpPatch("{id:int}/TrangThai")]
        public async Task<IActionResult> UpdateTrangThai(int id, [FromBody] UpdateTrangThaiRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.TrangThai))
                return BadRequest("Thiếu trạng thái.");

            var ok = await _repository.UpdateTrangThaiAsync(id, req.TrangThai, req.LyDoHuy);
            return ok ? NoContent() : NotFound();
        }
    }
}
