using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangChiTiet_ToppingController : ControllerBase
    {
        private readonly IGioHangChiTiet_ToppingRepository _repository;

        public GioHangChiTiet_ToppingController(IGioHangChiTiet_ToppingRepository repository)
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
        public async Task<IActionResult> Create(GioHangChiTiet_Topping model)
        {
            await _repository.AddAsync(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(GioHangChiTiet_Topping model)
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