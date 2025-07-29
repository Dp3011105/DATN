using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangVoucherController : ControllerBase
    {
        private readonly IKhachHangVoucherRepository _repository;

        public KhachHangVoucherController(IKhachHangVoucherRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KhachHangVoucher model)
        {
            await _repository.AddAsync(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(KhachHangVoucher model)
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
}