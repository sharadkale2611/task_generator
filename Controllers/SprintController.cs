using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SprintController : ControllerBase
	{
		private readonly ISprintService _sprintService;

		public SprintController(ISprintService sprintService)
		{
			_sprintService = sprintService;
		}

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromQuery] int epicId)
        {
            try
            {
                var result = await _sprintService.GenerateAsync(epicId);
                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Sprints generated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // 👈 show actual error
            }
        }

        [HttpGet("project/{epicId}")]
        public async Task<IActionResult> GetByProject(int epicId)
        {
            try
            {
                var result = await _sprintService.GetByEpicIdAsync(epicId);

                if (!result.Any())
                {
                    return Ok(HttpResponseDto<object>.SuccessResponse(
                        result,
                        "No sprints found for this epic"
                    ));
                }

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return NotFound(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }
    }
}
