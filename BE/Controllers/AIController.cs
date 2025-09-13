using BE.Data;
using BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIChatController : ControllerBase
    {
        private readonly AIChatService _aiChatService;

        public AIChatController(AIChatService aiChatService)
        {
            _aiChatService = aiChatService;
        }

        [HttpPost("chat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Chat([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Query))
            {
                return BadRequest("Query is required.");
            }

            // Sử dụng biến trung gian để tránh lỗi deconstruction
            var result = await _aiChatService.GenerateResponsesAsync(request.Query);
            var textResponses = result.TextResponses;
            var jsonResponses = result.JsonResponses;

            return Ok(new
            {
                TextResponses = textResponses,
                JsonResponses = jsonResponses
            });
        }
    }

    public class ChatRequest
    {
        public string Query { get; set; } = string.Empty;
    }
}