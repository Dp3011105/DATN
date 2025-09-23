using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        [HttpGet("{khachHangId}")]
        public async Task<IActionResult> GetProfile(int khachHangId)
        {
            try
            {
                var khachHang = await _profileRepository.GetKhachHangByIdAsync(khachHangId);
                if (khachHang == null)
                {
                    return NotFound("Không tìm thấy khách hàng.");
                }

                var diaChis = await _profileRepository.GetDiaChiByKhachHangIdAsync(khachHangId);

                var response = new ProfileResponse
                {
                    ID_Khach_Hang = khachHang.ID_Khach_Hang,
                    Ho_Ten = khachHang.Ho_Ten,
                    Email = khachHang.Email,
                    So_Dien_Thoai = khachHang.So_Dien_Thoai,
                    GioiTinh = khachHang.GioiTinh,
                    Ghi_Chu = khachHang.Ghi_Chu,
                    DiaChis = diaChis.Select(dc => new AddressResponse
                    {
                        ID_Dia_Chi = dc.ID_Dia_Chi,
                        Dia_Chi = dc.Dia_Chi,
                        Tinh_Thanh = dc.Tinh_Thanh,
                        Ghi_Chu = dc.Ghi_Chu,
                        Trang_Thai = dc.Trang_Thai
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        //[HttpPut("{khachHangId}")]
        //public async Task<IActionResult> UpdateProfile(int khachHangId, [FromBody] ProfileUpdateRequest request)
        //{
        //    try
        //    {
        //        var khachHang = await _profileRepository.GetKhachHangByIdAsync(khachHangId);
        //        if (khachHang == null)
        //        {
        //            return NotFound("Không tìm thấy khách hàng.");
        //        }

        //        // Kiểm tra email đã tồn tại hay chưa (nếu email thay đổi)
        //        if (!string.IsNullOrEmpty(request.Email) && request.Email != khachHang.Email)
        //        {
        //            var existingCustomer = await _profileRepository.GetKhachHangByEmailAsync(request.Email);
        //            if (existingCustomer != null && existingCustomer.ID_Khach_Hang != khachHangId)
        //            {
        //                return BadRequest("Email này đã được sử dụng bởi tài khoản khác.");
        //            }
        //        }

        //        // Update all fields including email
        //        khachHang.Ho_Ten = request.Ho_Ten;
        //        khachHang.Email = request.Email; // Cho phép cập nhật email
        //        khachHang.So_Dien_Thoai = request.So_Dien_Thoai;
        //        khachHang.GioiTinh = request.GioiTinh;
        //        khachHang.Ghi_Chu = request.Ghi_Chu;

        //        var success = await _profileRepository.UpdateKhachHangAsync(khachHang);
        //        if (!success)
        //        {
        //            return BadRequest("Không thể cập nhật thông tin.");
        //        }

        //        await _profileRepository.SaveChangesAsync();
        //        return Ok("Cập nhật thông tin thành công.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi server: {ex.Message}");
        //    }
        //}

        //fix cái ghi chú null , để không phải đổi model , không để ghi chú ở view nữa nhá , cáu 
        [HttpPut("{khachHangId}")]
        public async Task<IActionResult> UpdateProfile(int khachHangId, [FromBody] ProfileUpdateRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var khachHang = await _profileRepository.GetKhachHangByIdAsync(khachHangId);
                if (khachHang == null)
                {
                    return NotFound("Không tìm thấy khách hàng.");
                }

                // Kiểm tra email đã tồn tại hay chưa (chỉ khi thay đổi email)
                if (!string.IsNullOrWhiteSpace(request.Email) &&
                    !request.Email.Equals(khachHang.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingCustomer = await _profileRepository.GetKhachHangByEmailAsync(request.Email);
                    if (existingCustomer != null && existingCustomer.ID_Khach_Hang != khachHangId)
                    {
                        return BadRequest("Email này đã được sử dụng bởi tài khoản khác.");
                    }
                }

                // Cập nhật thông tin khách hàng
                khachHang.Ho_Ten = request.Ho_Ten?.Trim();
                khachHang.Email = request.Email?.Trim();
                khachHang.So_Dien_Thoai = request.So_Dien_Thoai?.Trim();
                khachHang.GioiTinh = request.GioiTinh;
                khachHang.Ghi_Chu = string.IsNullOrWhiteSpace(request.Ghi_Chu)
                                            ? "Không có"
                                            : request.Ghi_Chu.Trim();

                var success = await _profileRepository.UpdateKhachHangAsync(khachHang);
                if (!success)
                {
                    return BadRequest("Không thể cập nhật thông tin.");
                }

                await _profileRepository.SaveChangesAsync();
                return Ok("Cập nhật thông tin thành công.");
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu có hệ thống logging)
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }




        [HttpPost("{khachHangId}/address")]
        public async Task<IActionResult> AddAddress(int khachHangId, [FromBody] AddressCreateRequest request)
        {
            try
            {
                var diaChi = new DiaChi
                {
                    Dia_Chi = request.Dia_Chi,
                    Tinh_Thanh = "Hà Nội",
                    Ghi_Chu = request.Ghi_Chu,
                    Trang_Thai = true
                };

                var success = await _profileRepository.AddDiaChiAsync(diaChi, khachHangId);
                if (!success)
                {
                    return BadRequest("Không thể thêm địa chỉ.");
                }

                await _profileRepository.SaveChangesAsync();
                return Ok("Thêm địa chỉ thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpPut("{khachHangId}/address/{diaChiId}")]
        public async Task<IActionResult> UpdateAddress(int khachHangId, int diaChiId, [FromBody] AddressUpdateRequest request)
        {
            try
            {
                var diaChi = await _profileRepository.GetDiaChiByIdAsync(diaChiId);
                if (diaChi == null)
                {
                    return NotFound("Không tìm thấy địa chỉ.");
                }

                diaChi.Dia_Chi = request.Dia_Chi;
                diaChi.Tinh_Thanh = "Hà Nội";
                diaChi.Ghi_Chu = request.Ghi_Chu;
                diaChi.Trang_Thai = request.Trang_Thai;

                var success = await _profileRepository.UpdateDiaChiAsync(diaChi);
                if (!success)
                {
                    return BadRequest("Không thể cập nhật địa chỉ.");
                }

                await _profileRepository.SaveChangesAsync();
                return Ok("Cập nhật địa chỉ thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpDelete("{khachHangId}/address/{diaChiId}")]
        public async Task<IActionResult> DeleteAddress(int khachHangId, int diaChiId)
        {
            try
            {
                var success = await _profileRepository.DeleteDiaChiAsync(diaChiId, khachHangId);
                if (!success)
                {
                    return BadRequest("Không thể xóa địa chỉ.");
                }

                await _profileRepository.SaveChangesAsync();
                return Ok("Xóa địa chỉ thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
    }
}