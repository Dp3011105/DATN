using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiRepository _khuyenMaiRepository;

        public KhuyenMaiController(IKhuyenMaiRepository khuyenMaiRepository)
        {
            _khuyenMaiRepository = khuyenMaiRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<KhuyenMaiDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllKhuyenMai()
        {
            try
            {
                var khuyenMais = await _khuyenMaiRepository.GetAllAsync();
                return Ok(khuyenMais);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(KhuyenMaiDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetKhuyenMaiById(int id)
        {
            try
            {
                var khuyenMai = await _khuyenMaiRepository.GetByIdAsync(id);
                if (khuyenMai == null)
                {
                    return NotFound(new { message = "Khuyến mãi không tồn tại" });
                }
                return Ok(khuyenMai);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(KhuyenMaiDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateKhuyenMai([FromBody] KhuyenMaiDTO khuyenMaiDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var created = await _khuyenMaiRepository.CreateAsync(khuyenMaiDTO);
                var createdDTO = new KhuyenMaiDTO
                {
                    ID_Khuyen_Mai = created.ID_Khuyen_Mai,
                    Ten_Khuyen_Mai = created.Ten_Khuyen_Mai,
                    Ngay_Bat_Dau = created.Ngay_Bat_Dau,
                    Ngay_Ket_Thuc = created.Ngay_Ket_Thuc,
                    Mo_Ta = created.Mo_Ta,
                    Trang_Thai = created.Trang_Thai
                };
                return CreatedAtAction(nameof(GetKhuyenMaiById), new { id = created.ID_Khuyen_Mai }, createdDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(KhuyenMaiDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateKhuyenMai(int id, [FromBody] KhuyenMaiDTO khuyenMaiDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await _khuyenMaiRepository.UpdateAsync(id, khuyenMaiDTO);
                if (updated == null)
                {
                    return NotFound(new { message = "Khuyến mãi không tồn tại" });
                }
                var updatedDTO = new KhuyenMaiDTO
                {
                    ID_Khuyen_Mai = updated.ID_Khuyen_Mai,
                    Ten_Khuyen_Mai = updated.Ten_Khuyen_Mai,
                    Ngay_Bat_Dau = updated.Ngay_Bat_Dau,
                    Ngay_Ket_Thuc = updated.Ngay_Ket_Thuc,
                    Mo_Ta = updated.Mo_Ta,
                    Trang_Thai = updated.Trang_Thai
                };
                return Ok(updatedDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.Message });
            }
        }

    }
}
