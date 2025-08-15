using BE.DTOs;
using BE.models;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using System.Net;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanVaiTroController : ControllerBase
    {
        private readonly ITaiKhoanVaiTroRepository _repository;

        public TaiKhoanVaiTroController(ITaiKhoanVaiTroRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaiKhoanVaiTroDTO>>> GetAllTaiKhoanVaiTro()
        {
            var result = await _repository.GetAllTaiKhoanVaiTroAsync();
            return Ok(result);
        }
        [HttpGet("{idTaiKhoan}/{idVaiTro}")]
        public async Task<ActionResult<TaiKhoanVaiTroDTO>> GetTaiKhoanVaiTroById(int idTaiKhoan, int idVaiTro)
        {
            var result = await _repository.GetTaiKhoanVaiTroByIdAsync(idTaiKhoan, idVaiTro);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> CreateTaiKhoanVaiTro([FromBody] TaiKhoanVaiTro taiKhoanVaiTro)
        {
            if (taiKhoanVaiTro == null)
            {
                return BadRequest("Invalid data.");
            }
            await _repository.CreateTaiKhoanVaiTroAsync(taiKhoanVaiTro);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut("{idTaiKhoan}/{idVaiTro}")]
        public async Task<ActionResult> UpdateTaiKhoanVaiTro(int idTaiKhoan, int idVaiTro, [FromBody] TaiKhoanVaiTro taiKhoanVaiTro)
        {
            if (taiKhoanVaiTro == null)
            {
                return BadRequest("Invalid data.");
            }
            await _repository.UpdateTaiKhoanVaiTroAsync(taiKhoanVaiTro);
            return NoContent();
        }
        [HttpDelete("{idTaiKhoan}/{idVaiTro}")]
        public async Task<ActionResult> DeleteTaiKhoanVaiTro(int idTaiKhoan, int idVaiTro)
        {
            var taiKhoanVaiTro = await _repository.GetTaiKhoanVaiTroByIdAsync(idTaiKhoan, idVaiTro);
            if (taiKhoanVaiTro == null)
            {
                return NotFound();
            }
            await _repository.DeleteTaiKhoanVaiTroAsync(idTaiKhoan, idVaiTro);
            return NoContent();
        }
    }
}