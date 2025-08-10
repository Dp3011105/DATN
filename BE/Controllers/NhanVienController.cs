using BE.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienRepository _repository;
        public NhanVienController(INhanVienRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public ActionResult GetAllNv() {
            try
            {
                var result = _repository.GetAllAsync().Result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public ActionResult ChiTietNv() {
            try
            {
                var id = int.Parse(HttpContext.Request.RouteValues["id"].ToString());
                var result = _repository.GetByIdAsync(id).Result;
                return Ok(result);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost]
        public ActionResult ThemNv([FromBody] NhanVien entity)
        {
            if (entity == null)
            {
                return BadRequest("Entity cannot be null.");
            }

            try
            {
                _repository.AddAsync(entity);
                return CreatedAtAction(nameof(ChiTietNv), new { id = entity.ID_Nhan_Vien }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public ActionResult SuaNv(int id, [FromBody] NhanVien entity)
        {
            if (entity == null || entity.ID_Nhan_Vien != id)
            {
                return BadRequest("Entity cannot be null and ID must match.");
            }

            try
            {
                _repository.UpdateAsync(id, entity).Wait();
                return Ok();
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult XoaNv(int id)
        {
            try
            {
                _repository.DeleteAsync(id).Wait();
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}