using BE.DTOs;
using BE.models;
using BE.Repository;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuongDaController : ControllerBase
    {
        private readonly ILuongDaRepository _luongDaRepository;

        public LuongDaController(ILuongDaRepository luongDaRepository)
        {
            _luongDaRepository = luongDaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<LuongDaDTO>>> GetAllLuongDa()
        {
            var luongDas = await _luongDaRepository.GetAllAsync();
            return Ok(luongDas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LuongDaDTO>> GetLuongDaById(int id)
        {
            var luongDa = await _luongDaRepository.GetByIdAsync(id);
            if (luongDa == null)
            {
                return NotFound(new { message = "Lượng đá không tồn tại" });
            }
            return Ok(luongDa);
        }

        [HttpPost]
        public async Task<ActionResult<LuongDaDTO>> CreateLuongDa([FromBody] LuongDaDTO luongDaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdLuongDa = await _luongDaRepository.CreateAsync(luongDaDTO);
            return CreatedAtAction(nameof(GetLuongDaById), new { id = createdLuongDa.ID_LuongDa }, createdLuongDa);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LuongDaDTO>> UpdateLuongDa(int id, [FromBody] LuongDaDTO luongDaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != luongDaDTO.ID_LuongDa)
            {
                return BadRequest(new { message = "ID lượng đá không khớp" });
            }

            var updatedLuongDa = await _luongDaRepository.UpdateAsync(id, luongDaDTO);
            if (updatedLuongDa == null)
            {
                return NotFound(new { message = "Lượng đá không tồn tại" });
            }

            return Ok(updatedLuongDa);
        }
    }
}