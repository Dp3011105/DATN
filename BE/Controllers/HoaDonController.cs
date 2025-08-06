using BE.models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class HoaDonController : ControllerBase
{
    private readonly IHoaDonRepository _repository;

    public HoaDonController(IHoaDonRepository repository) => _repository = repository;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(HoaDonCreateDTO dto)
    {
        var model = new HoaDon
        {
            ID_Khach_Hang = dto.ID_Khach_Hang,
            ID_Nhan_Vien = dto.ID_Nhan_Vien,
            ID_Hinh_Thuc_Thanh_Toan = dto.ID_Hinh_Thuc_Thanh_Toan,
            ID_Dia_Chi = dto.ID_Dia_Chi,
            ID_Phi_Ship = dto.ID_Phi_Ship,
            Dia_Chi_Tu_Nhap = dto.Dia_Chi_Tu_Nhap,
            Ngay_Tao = dto.Ngay_Tao,
            Tong_Tien = dto.Tong_Tien,
            Phi_Ship = dto.Phi_Ship,
            Trang_Thai = dto.Trang_Thai,
            Ghi_Chu = dto.Ghi_Chu,
            Ma_Hoa_Don = dto.Ma_Hoa_Don,
            Loai_Hoa_Don = dto.Loai_Hoa_Don,
            LyDoHuyDon = dto.LyDoHuyDon,
            LyDoDonHangCoVanDe = dto.LyDoDonHangCoVanDe
        };

        await _repository.AddAsync(model);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(HoaDon model)
    {
        await _repository.UpdateAsync(model);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}
