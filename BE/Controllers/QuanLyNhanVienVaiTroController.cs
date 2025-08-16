using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyNhanVienVaiTroController : ControllerBase
    {

        private readonly IQuanLyPhanQuyenNhanVienRepository _repository;

        public QuanLyNhanVienVaiTroController(IQuanLyPhanQuyenNhanVienRepository repository)
        {
            _repository = repository;
        }



        // API: Get all employee accounts with roles
        [HttpGet("nhan-vien-co-vai-tro")]
        public async Task<ActionResult<List<NhanVienVoiTaiKhoan>>> LayNhanVienCoVaiTro()
        {
            var result = await _repository.LayDanhSachNhanVienCoVaiTroAsync();
            return Ok(result);
        }

        // API: Get all employee accounts without roles
        [HttpGet("nhan-vien-khong-vai-tro")]
        public async Task<ActionResult<List<NhanVienVoiTaiKhoan>>> LayNhanVienKhongVaiTro()
        {
            var result = await _repository.LayDanhSachNhanVienKhongCoVaiTroAsync();
            return Ok(result);
        }

        // API: Assign roles to an account (allows multiple roles)
        [HttpPost("gan-vai-tro")]
        public async Task<ActionResult> GanVaiTro([FromBody] GanVaiTroYeuCauDTO request)
        {
            if (request == null || !request.Danh_Sach_ID_Vai_Tro.Any())
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                await _repository.GanVaiTroChoTaiKhoanAsync(request.ID_Tai_Khoan, request.Danh_Sach_ID_Vai_Tro);
                return Ok("Vai trò đã được thêm thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // API: Update roles for an account (allows adding and removing roles)
        [HttpPut("cap-nhat-vai-tro")]
        public async Task<ActionResult> CapNhatVaiTro([FromBody] GanVaiTroYeuCauDTO request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                await _repository.CapNhatVaiTroChoTaiKhoanAsync(request.ID_Tai_Khoan, request.Danh_Sach_ID_Vai_Tro);
                return Ok("Vai trò đã được cập nhật thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
