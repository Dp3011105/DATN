using BE.DTOs;
using BE.models;
using BE.Repository.IRepository;
using BE.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangTKController : ControllerBase
    {


        private readonly IDonHangTKRepository _repo;
        private readonly EmailService _emailService;
        public DonHangTKController(EmailService emailService,IDonHangTKRepository repo)
        {
            _emailService = emailService;
            _repo = repo;
        }

        [HttpGet("{idKhachHang}")]
        public async Task<IActionResult> GetDonHangs(int idKhachHang)
        {
            var hoaDons = await _repo.GetHoaDonsByKhachHangAsync(idKhachHang);
            var dtoList = hoaDons.Select(h => MapToHoaDonDonHangTKDTO(h)).ToList();
            return Ok(dtoList);
        }

        //[HttpPost("huy")]
        //public async Task<IActionResult> HuyDon([FromBody] HuyDonHangTKDTO request)
        //{
        //    if (string.IsNullOrEmpty(request.LyDoHuyDon))
        //    {
        //        return BadRequest("Lý do hủy đơn là bắt buộc");
        //    }

        //    var hoaDon = await _repo.GetHoaDonByIdAsync(request.ID_Hoa_Don);
        //    if (hoaDon == null)
        //    {
        //        return NotFound("Hóa đơn không tìm thấy");
        //    }

        //    if (hoaDon.ID_Khach_Hang != request.ID_Khach_Hang)
        //    {
        //        return Unauthorized("Không phải đơn hàng của bạn");
        //    }

        //    if (hoaDon.Trang_Thai != "Chua_Xac_Nhan")
        //    {
        //        return BadRequest("Không thể hủy, trạng thái đơn hàng không phải Chua_Xac_Nhan");
        //    }

        //    hoaDon.Trang_Thai = "Huy_Don";
        //    hoaDon.LyDoHuyDon = request.LyDoHuyDon;
        //    await _repo.UpdateHoaDonAsync(hoaDon);

        //    return Ok("Hủy đơn hàng thành công");
        //}

        [HttpPost("huy")]
        public async Task<IActionResult> HuyDon([FromBody] HuyDonHangTKDTO request)
        {
            if (string.IsNullOrEmpty(request.LyDoHuyDon))
            {
                return BadRequest("Lý do hủy đơn là bắt buộc");
            }

            var hoaDon = await _repo.GetHoaDonByIdAsync(request.ID_Hoa_Don);
            if (hoaDon == null)
            {
                return NotFound("Hóa đơn không tìm thấy");
            }

            if (hoaDon.ID_Khach_Hang != request.ID_Khach_Hang)
            {
                return Unauthorized("Không phải đơn hàng của bạn");
            }

            if (hoaDon.Trang_Thai != "Chua_Xac_Nhan")
            {
                return BadRequest("Không thể hủy, trạng thái đơn hàng không phải Chua_Xac_Nhan");
            }

            // Kiểm tra xem đơn hàng có sử dụng voucher không
            var hoaDonVoucher = await _repo.GetHoaDonVoucherByHoaDonIdAsync(request.ID_Hoa_Don);
            if (hoaDonVoucher != null)
            {
                // Nếu có voucher, kiểm tra trạng thái trong KhachHangVoucher
                var khachHangVoucher = await _repo.GetKhachHangVoucherAsync(hoaDon.ID_Khach_Hang.Value, hoaDonVoucher.ID_Voucher);
                if (khachHangVoucher != null && khachHangVoucher.Trang_Thai == false)
                {
                    // Cập nhật trạng thái thành true nếu đang là false
                    khachHangVoucher.Trang_Thai = true;
                    await _repo.UpdateKhachHangVoucherAsync(khachHangVoucher);
                }
            }

            // Cập nhật trạng thái đơn hàng và lý do hủy
            hoaDon.Trang_Thai = "Huy_Don";
            hoaDon.LyDoHuyDon = request.LyDoHuyDon;
            await _repo.UpdateHoaDonAsync(hoaDon);

            return Ok("Hủy đơn hàng thành công");
        }




        //private HoaDonDonHangTKDTO MapToHoaDonDonHangTKDTO(HoaDon h)
        //{
        //    return new HoaDonDonHangTKDTO
        //    {
        //        ID_Hoa_Don = h.ID_Hoa_Don,
        //        Ma_Hoa_Don = h.Ma_Hoa_Don,
        //        Ngay_Tao = h.Ngay_Tao,
        //        Tong_Tien = h.Tong_Tien,
        //        Trang_Thai = h.Trang_Thai,
        //        Ghi_Chu = h.Ghi_Chu,
        //        Loai_Hoa_Don = h.Loai_Hoa_Don,
        //        LyDoHuyDon = h.LyDoHuyDon,
        //        LyDoDonHangCoVanDe = h.LyDoDonHangCoVanDe,
        //        Phuong_Thuc_Thanh_Toan = h.HinhThucThanhToan?.Phuong_Thuc_Thanh_Toan,
        //        DiaChi = h.DiaChi != null ? new DiaChiDonHangTK
        //        {
        //            Dia_Chi = h.DiaChi.Dia_Chi,
        //            Tinh_Thanh = h.DiaChi.Tinh_Thanh
        //        } : null,
        //        ChiTiets = h.HoaDonChiTiets.Select(ct => new HoaDonChiTietDonHangTKDTO
        //        {
        //            ID_HoaDon_ChiTiet = ct.ID_HoaDon_ChiTiet,
        //            Ten_San_Pham = ct.SanPham?.Ten_San_Pham,
        //            SizeName = ct.Size?.SizeName,
        //            Muc_Do = ct.DoNgot?.Muc_Do,
        //            Ten_LuongDa = ct.LuongDa?.Ten_LuongDa,
        //            Toppings = ct.HoaDonChiTietToppings.Select(ht => new ToppingDonHangTKDTO
        //            {
        //                Ten = ht.Topping?.Ten,
        //                Gia = ht.Topping?.Gia ?? 0
        //            }).ToList(),
        //            So_Luong = ct.So_Luong,
        //            Tong_Tien = ct.Tong_Tien,
        //            Ghi_Chu = ct.Ghi_Chu
        //        }).ToList(),
        //        Vouchers = h.HoaDonVouchers.Select(hv => new VoucherDonHangTKDTO
        //        {
        //            Ma_Voucher = hv.Voucher?.Ma_Voucher,
        //            Ten = hv.Voucher?.Ten,
        //            Gia_Tri_Giam = hv.Voucher?.Gia_Tri_Giam
        //        }).ToList()
        //    };
        //}


        private HoaDonDonHangTKDTO MapToHoaDonDonHangTKDTO(HoaDon h)
        {
            return new HoaDonDonHangTKDTO
            {
                ID_Hoa_Don = h.ID_Hoa_Don,
                Ma_Hoa_Don = h.Ma_Hoa_Don,
                Ngay_Tao = h.Ngay_Tao,
                Tong_Tien = h.Tong_Tien,
                Trang_Thai = h.Trang_Thai,
                Ghi_Chu = h.Ghi_Chu,
                Loai_Hoa_Don = h.Loai_Hoa_Don,
                LyDoHuyDon = h.LyDoHuyDon,
                LyDoDonHangCoVanDe = h.LyDoDonHangCoVanDe,
                Phuong_Thuc_Thanh_Toan = h.HinhThucThanhToan?.Phuong_Thuc_Thanh_Toan,
                DiaChi = h.DiaChi != null ? new DiaChiDonHangTK
                {
                    Dia_Chi = h.DiaChi.Dia_Chi,
                    Tinh_Thanh = h.DiaChi.Tinh_Thanh,
                    Ghi_Chu = h.DiaChi.Ghi_Chu

                } : null,
                ChiTiets = h.HoaDonChiTiets.Select(ct => new HoaDonChiTietDonHangTKDTO
                {
                    ID_HoaDon_ChiTiet = ct.ID_HoaDon_ChiTiet,
                    Ten_San_Pham = ct.SanPham?.Ten_San_Pham,
                    SizeName = ct.Size?.SizeName,
                    Muc_Do = ct.DoNgot?.Muc_Do,
                    Ten_LuongDa = ct.LuongDa?.Ten_LuongDa,
                    Toppings = ct.HoaDonChiTietToppings.Select(ht => new ToppingDonHangTKDTO
                    {
                        Ten = ht.Topping?.Ten,
                        Gia = ht.Topping?.Gia ?? 0
                    }).ToList(),
                    So_Luong = ct.So_Luong,
                    Tong_Tien = ct.Tong_Tien,
                    Ghi_Chu = ct.Ghi_Chu
                }).ToList(),
                Vouchers = h.HoaDonVouchers.Select(hv => new VoucherDonHangTKDTO
                {
                    Ma_Voucher = hv.Voucher?.Ma_Voucher,
                    Ten = hv.Voucher?.Ten,
                    Gia_Tri_Giam = hv.Voucher?.Gia_Tri_Giam
                }).ToList(),

                // Gán Phi_Ship
                Phi_Ship = h.Phi_Ship
            };
        }





    }
}
