using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;


namespace BE.Controllers
{
    // TRƯỚC KHI dùng chức năng thì trong bảng VaiTro phải có ít nhất 1 vai trò với ID_Vai_Tro = 1 (Khách hàng) 
    // Chức năng đăng ký sẽ được hoạt động là người dùng nhập các dữ liệu , gmail các thứ ,
    // sau đó chương trình sẽ tự động tạo tài khoản cho người dùng đó, và gửi mật khẩu về email của người dùng đó
    //Chức năng đăng nhập chỉ cần nhập tên đăng nhập và mật khẩu, nếu đúng thì sẽ trả về thông tin tài khoản, nếu sai thì sẽ báo lỗi
    //dạng json khi khách hàng được đăng nhập thành công sẽ có dạng như sau: ( Vai trò khách hàng sẽ tạm thời sẽ để là 1 )

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly EmailService _emailService;

        public AuthController(ICustomerRepository customerRepository,
                             IAccountRepository accountRepository,
                             EmailService emailService)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _emailService = emailService;
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }
            return password.ToString();
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] BE.DTOs.RegisterRequest request)
        //{
        //    // Kiểm tra email hoặc tên người dùng đã tồn tại
        //    if (await _customerRepository.EmailExistsAsync(request.Email))
        //    {
        //        return BadRequest("Email đã tồn tại.");
        //    }
        //    if (await _accountRepository.UsernameExistsAsync(request.Ten_Nguoi_Dung))
        //    {
        //        return BadRequest("Tên người dùng đã tồn tại.");
        //    }

        //    // Tạo khách hàng mới
        //    var khachHang = new KhachHang
        //    {
        //        Email = request.Email,
        //        Ho_Ten = request.Ho_Ten,
        //        So_Dien_Thoai = "",
        //        Ghi_Chu = "",
        //        Trang_Thai = true
        //    };
        //    await _customerRepository.AddKhachHangAsync(khachHang);
        //    await _customerRepository.SaveChangesAsync();

        //    // Tạo mật khẩu ngẫu nhiên
        //    var password = GenerateRandomPassword();

        //    // Tạo tài khoản mới
        //    var taiKhoan = new TaiKhoan
        //    {
        //        ID_Khach_Hang = khachHang.ID_Khach_Hang,
        //        Ten_Nguoi_Dung = request.Ten_Nguoi_Dung,
        //        Mat_Khau = password,
        //        Trang_Thai = true
        //    };
        //    await _accountRepository.AddTaiKhoanAsync(taiKhoan);
        //    await _accountRepository.SaveChangesAsync();

        //    // Gán vai trò mặc định (ID_Vai_Tro = 1) 
        //    var taiKhoanVaiTro = new TaiKhoanVaiTro
        //    {
        //        ID_Tai_Khoan = taiKhoan.ID_Tai_Khoan,
        //        ID_Vai_Tro = 1 // có thể sửa lại nhó em yêu 
        //    };
        //    await _accountRepository.AddTaiKhoanVaiTroAsync(taiKhoanVaiTro);
        //    await _accountRepository.SaveChangesAsync();

        //    // Gửi email chứa mật khẩu
        //    var emailBody = $"Chào mừng bạn đến với Trà sữa TheBoy!\n\nTên người dùng: {request.Ten_Nguoi_Dung}\nMật khẩu: {password}";
        //    await _emailService.SendEmailAsync(request.Email, "Đăng ký tài khoản DATN", emailBody);

        //    return Ok("Đăng ký thành công. Vui lòng kiểm tra email để lấy mật khẩu.");
        //}




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] BE.DTOs.RegisterRequest request)
        {
            // Kiểm tra email có hợp lệ không
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Email không được để trống.");
            }

            // Kiểm tra mật khẩu có hợp lệ không
            if (string.IsNullOrWhiteSpace(request.Mat_Khau))
            {
                return BadRequest("Vui lòng nhập mật khẩu.");
            }

            // Kiểm tra email hoặc tên người dùng đã tồn tại
            if (await _customerRepository.EmailExistsAsync(request.Email))
            {
                return BadRequest("Email đã tồn tại.");
            }
            if (await _accountRepository.UsernameExistsAsync(request.Ten_Nguoi_Dung))
            {
                return BadRequest("Tên người dùng đã tồn tại.");
            }

            // Tạo khách hàng mới
            var khachHang = new KhachHang
            {
                Email = request.Email, // Lưu email vào bảng KhachHang
                Ho_Ten = request.Ho_Ten,
                So_Dien_Thoai = "",
                Ghi_Chu = "",
                Trang_Thai = true
            };
            await _customerRepository.AddKhachHangAsync(khachHang);
            await _customerRepository.SaveChangesAsync();

            // Tạo tài khoản mới với email đồng bộ từ KhachHang
            var taiKhoan = new TaiKhoan
            {
                ID_Khach_Hang = khachHang.ID_Khach_Hang,
                Ten_Nguoi_Dung = request.Ten_Nguoi_Dung,
                Mat_Khau = request.Mat_Khau, // Sử dụng mật khẩu từ request
                Email = khachHang.Email, // Đồng bộ email từ KhachHang
                Trang_Thai = true
            };
            await _accountRepository.AddTaiKhoanAsync(taiKhoan);
            await _accountRepository.SaveChangesAsync();

            // Gán vai trò mặc định (ID_Vai_Tro = 1)
            var taiKhoanVaiTro = new TaiKhoanVaiTro
            {
                ID_Tai_Khoan = taiKhoan.ID_Tai_Khoan,
                ID_Vai_Tro = 1 // Vai trò mặc định, có thể chỉnh sửa nếu cần
            };
            await _accountRepository.AddTaiKhoanVaiTroAsync(taiKhoanVaiTro);
            await _accountRepository.SaveChangesAsync();

            // Gửi email chào mừng
            var emailBody = $"Chào mừng bạn đến với Trà Sữa TheBoy!\n\nTên người dùng: {request.Ten_Nguoi_Dung}\nCảm ơn bạn đã đăng ký tài khoản.";
            await _emailService.SendEmailAsync(khachHang.Email, "Chào mừng đến với Trà Sữa TheBoy", emailBody);

            return Ok("Bạn đã đăng ký thành công , Chào mừng bạn đến với Trà Sữa TheBoy ");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            // Kiểm tra thông tin đăng nhập
            var taiKhoan = await _accountRepository.GetTaiKhoanByUsernameAsync(request.Ten_Nguoi_Dung);
            if (taiKhoan == null || taiKhoan.Mat_Khau != request.Mat_Khau)
            {
                return BadRequest("Tên người dùng hoặc mật khẩu không đúng.");
            }

            // Kiểm tra trạng thái tài khoản    
            if (!taiKhoan.Trang_Thai)
            {
                return BadRequest("Tài khoản đã bị khóa.");
            }

           
            if (taiKhoan.ID_Khach_Hang == null && taiKhoan.ID_Nhan_Vien == null)
            {
                return BadRequest("Dữ liệu tài khoản này xung đột: Cả ID_Khach_Hang và ID_Nhan_Vien đều không tồn tại.");
            }

            // Lấy danh sách ID vai trò
            var vaiTros = await _accountRepository.GetVaiTroIdsByTaiKhoanIdAsync(taiKhoan.ID_Tai_Khoan);
            if (vaiTros == null || !vaiTros.Any())
            {
                return BadRequest("Tài khoản không có vai trò nào được gán.");
            }

            // Tạo response
            var response = new LoginResponseDTO
            {
                ID_Khach_Hang = taiKhoan.ID_Khach_Hang,
                ID_Nhan_Vien = taiKhoan.ID_Nhan_Vien,
                VaiTros = vaiTros
            };

            return Ok(response);
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var taiKhoan = await _accountRepository.GetTaiKhoanByKhachHangIdAsync(request.IdKhachHang);
            if (taiKhoan == null)
            {
                return BadRequest("Không tìm thấy tài khoản.");
            }

            if (taiKhoan.Mat_Khau != request.OldPassword)
            {
                return BadRequest("Mật khẩu cũ không đúng.");
            }

            taiKhoan.Mat_Khau = request.NewPassword;
            await _accountRepository.UpdateTaiKhoanAsync(taiKhoan);
            await _accountRepository.SaveChangesAsync();

            return Ok("Đổi mật khẩu thành công.");
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] BE.DTOs.CustomForgotPasswordRequest request)
        {
            var khachHang = await _customerRepository.GetKhachHangByEmailAsync(request.Email);
            if (khachHang == null)
            {
                return BadRequest("Email không tồn tại.");
            }

            var taiKhoan = await _accountRepository.GetTaiKhoanByKhachHangIdAsync(khachHang.ID_Khach_Hang);
            if (taiKhoan == null)
            {
                return BadRequest("Không tìm thấy tài khoản liên kết.");
            }

            var newPassword = GenerateRandomPassword();
            taiKhoan.Mat_Khau = newPassword;
            await _accountRepository.UpdateTaiKhoanAsync(taiKhoan);
            await _accountRepository.SaveChangesAsync();

            var emailBody = $"Yêu cầu quên mật khẩu của bạn đã được xử lý.\n\nMật khẩu mới: {newPassword}\nVui lòng đổi mật khẩu sau khi đăng nhập.";
            await _emailService.SendEmailAsync(request.Email, "Quên mật khẩu DATN", emailBody);

            return Ok("Mật khẩu mới đã được gửi qua email.");
        }

    }
}
