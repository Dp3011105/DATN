using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repository;
        private readonly EmailService _emailService;
        public HoaDonController(IHoaDonRepository repository, EmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
            => Ok(await _repository.GetAllAsync()); // DTO list (giữ như cũ)

        [HttpGet("entities")]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetAllEntities()
            => Ok(await _repository.GetAllEntitiesAsync());   // ⭐ đã Include KH & ĐC ở repo

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoaDon = await _repository.GetByIdAsync(id);
            if (hoaDon == null) return NotFound();
            return Ok(hoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null) return BadRequest("HoaDon cannot be null.");
            await _repository.AddAsync(hoaDon);
            return CreatedAtAction(nameof(GetById), new { id = hoaDon.ID_Hoa_Don }, hoaDon);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null || id != hoaDon.ID_Hoa_Don)
                return BadRequest("Invalid HoaDon data.");

            await _repository.UpdateAsync(id, hoaDon);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        public class UpdateTrangThaiRequest
        {
            public string TrangThai { get; set; } = default!;
            public string? LyDoHuy { get; set; }
        }

        [HttpPatch("{id:int}/TrangThai")]
        public async Task<IActionResult> UpdateTrangThai(int id, [FromBody] UpdateTrangThaiRequest req)
        {
            if (req == null || string.IsNullOrWhiteSpace(req.TrangThai))
                return BadRequest("Thiếu trạng thái.");

            var ok = await _repository.UpdateTrangThaiAsync(id, req.TrangThai, req.LyDoHuy, _emailService);
            return ok ? NoContent() : NotFound();
        }

        public class RestockItem
        {
            public int HoaDonChiTietId { get; set; }
            public int Quantity { get; set; }
        }

        public class CancelWithRestockRequest
        {
            public string LyDo { get; set; } = "";
            public List<RestockItem> Items { get; set; } = new();
        }

        [HttpPost("{id:int}/cancel-with-restock")]
        public async Task<IActionResult> CancelWithRestock(int id, [FromBody] CancelWithRestockRequest req)
        {
            if (id <= 0) return BadRequest("HoaDonId invalid.");
            var items = (req?.Items ?? new()).Select(x => (x.HoaDonChiTietId, x.Quantity));
            var ok = await _repository.CancelWithRestockAsync(id, req?.LyDo ?? "", items);
            return ok ? Ok(new { ok = true }) : BadRequest(new { ok = false });
        }
    }
}
