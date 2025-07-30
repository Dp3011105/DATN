using BE.DTOs;
using BE.models;
using BE.Repository;
using BE.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Repository.IRepository;


namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoNgotController : ControllerBase
    {
        private readonly IDoNgotRepository _doNgotRepository;

        public DoNgotController(IDoNgotRepository doNgotRepository)
        {
            _doNgotRepository = doNgotRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<DoNgotDTO>>> GetAllDoNgots()
        {
            var doNgots = await _doNgotRepository.GetAllAsync();
            return Ok(doNgots);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoNgotDTO>> GetDoNgotById(int id)
        {
            var doNgot = await _doNgotRepository.GetByIdAsync(id);
            if (doNgot == null)
            {
                return NotFound(new { message = "Độ ngọt không tồn tại" });
            }
            return Ok(doNgot);
        }

        [HttpPost]
        public async Task<ActionResult<DoNgotDTO>> CreateDoNgot([FromBody] DoNgotDTO doNgotDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdDoNgot = await _doNgotRepository.CreateAsync(doNgotDTO);
            return CreatedAtAction(nameof(GetDoNgotById), new { id = createdDoNgot.ID_DoNgot }, createdDoNgot);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DoNgotDTO>> UpdateDoNgot(int id, [FromBody] DoNgotDTO doNgotDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doNgotDTO.ID_DoNgot)
            {
                return BadRequest(new { message = "ID độ ngọt không khớp" });
            }

            var updatedDoNgot = await _doNgotRepository.UpdateAsync(id, doNgotDTO);
            if (updatedDoNgot == null)
            {
                return NotFound(new { message = "Độ ngọt không tồn tại" });
            }

            return Ok(updatedDoNgot);
        }
    }
}