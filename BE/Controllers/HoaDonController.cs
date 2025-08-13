using BE.models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class HoaDonController : ControllerBase
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repository;
         
        // tình anh em có vậy thôi 

        public HoaDonController(IHoaDonRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hoaDons = await _repository.GetAllAsync();
            return Ok(hoaDons);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hoaDon = await _repository.GetByIdAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            return Ok(hoaDon);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null)
            {
                return BadRequest("HoaDon cannot be null.");
            }
            await _repository.AddAsync(hoaDon);
            return CreatedAtAction(nameof(GetById), new { id = hoaDon.ID_Hoa_Don }, hoaDon);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null || id != hoaDon.ID_Hoa_Don)
            {
                return BadRequest("Invalid HoaDon data.");
            }
            var existingHoaDon = await _repository.GetByIdAsync(id);
            if (existingHoaDon == null)
            {
                return NotFound();
            }
            await _repository.UpdateAsync(id, hoaDon);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingHoaDon = await _repository.GetByIdAsync(id);
            if (existingHoaDon == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }

    }
}
