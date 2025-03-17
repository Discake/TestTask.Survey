using Microsoft.AspNetCore.Mvc;
using TestTask.Survey.Data.Dtos;
using TestTask.Survey.Services.Interfaces;

namespace TestTask.Survey.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsesController : ControllerBase
    {
        private readonly ISaveResponseService _responseService;

        public ResponsesController(ISaveResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(NextQuestionResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SaveResponse([FromBody] SaveAnswerRequestDto request)
        {
            try
            {
                return Ok(await _responseService.SaveResponseAsync(request));
            }
            catch (Exception ex)
            {
                switch (_responseService.ErrorCode)
                {
                    case 400:
                        return BadRequest(ex.Message);
                    case 404:
                        return NotFound(ex.Message);
                    default:
                        return StatusCode(500, "Internal server error");
                }
            }
        }
    }
}
