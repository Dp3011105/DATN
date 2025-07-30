
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
    public class SizeController : ControllerBase
    {

        private readonly ISizeRepository _sizeRepository;

        public SizeController(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SizeDTO>>> GetAllSizes()
        {
            var sizes = await _sizeRepository.GetAllAsync();
            return Ok(sizes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SizeDTO>> GetSizeById(int id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null)
            {
                return NotFound(new { message = "Size không tồn tại" });
            }
            return Ok(size);
        }

        [HttpPost]
        public async Task<ActionResult<SizeDTO>> CreateSize([FromBody] SizeDTO sizeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSize = await _sizeRepository.CreateAsync(sizeDTO);
            return CreatedAtAction(nameof(GetSizeById), new { id = createdSize.ID_Size }, createdSize);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SizeDTO>> UpdateSize(int id, [FromBody] SizeDTO sizeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sizeDTO.ID_Size)
            {
                return BadRequest(new { message = "ID size không khớp" });
            }

            var updatedSize = await _sizeRepository.UpdateAsync(id, sizeDTO);
            if (updatedSize == null)
            {
                return NotFound(new { message = "Size không tồn tại" });
            }

            return Ok(updatedSize);

        }
    }
}