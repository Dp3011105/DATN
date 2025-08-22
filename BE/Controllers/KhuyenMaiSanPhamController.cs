using BE.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiSanPhamController : ControllerBase
    {
        private readonly IKhuyenMaiSanPhamRepository _khuyenMaiSanPhamRepository;

        public KhuyenMaiSanPhamController(IKhuyenMaiSanPhamRepository khuyenMaiSanPhamRepository)
        {
            _khuyenMaiSanPhamRepository = khuyenMaiSanPhamRepository;
        }

        [HttpPost("assign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AssignKhuyenMaiToProducts([FromBody] AssignKhuyenMaiRequest request)
        {
            if (request.ID_Khuyen_Mai <= 0 || request.ID_San_Phams == null || request.ID_San_Phams.Count == 0 || request.Phan_Tram_Giam < 0 || request.Phan_Tram_Giam > 100)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });
            }

            try
            {
                await _khuyenMaiSanPhamRepository.AssignKhuyenMaiToProductsAsync(request.ID_Khuyen_Mai, request.ID_San_Phams, request.Phan_Tram_Giam);
                return Ok(new { message = "Gán khuyến mãi thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }






        // GET: api/KhuyenMaiSanPham/{idKhuyenMai}
        // GET: api/KhuyenMaiSanPham/{idKhuyenMai}
        [HttpGet("{idKhuyenMai}")]
        public async Task<ActionResult<IEnumerable<object>>> GetSanPhamByKhuyenMai(int idKhuyenMai)
        {
            try
            {
                var result = await _khuyenMaiSanPhamRepository.GetSanPhamByKhuyenMai(idKhuyenMai);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        // DELETE: api/KhuyenMaiSanPham/{idSanPham}/{idKhuyenMai}
        [HttpDelete("{idSanPham}/{idKhuyenMai}")]
        public async Task<ActionResult> HuyKhuyenMai(int idSanPham, int idKhuyenMai)
        {
            try
            {
                var result = await _khuyenMaiSanPhamRepository.HuyKhuyenMai(idSanPham, idKhuyenMai);
                if (!result)
                {
                    return NotFound("Không tìm thấy mối quan hệ khuyến mãi cho sản phẩm này.");
                }
                return Ok("Hủy khuyến mãi thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }




        // GET: api/KhuyenMaiSanPham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllSanPhamWithKhuyenMai()
        {
            try
            {
                var result = await _khuyenMaiSanPhamRepository.GetAllSanPhamWithKhuyenMai();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }


    }
}
