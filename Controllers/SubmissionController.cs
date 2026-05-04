using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionController : ControllerBase
    {
        private readonly ISubmissionService _service;

        public SubmissionController(ISubmissionService service)
        {
            _service = service;
        }

        // 🔹 Create Submission
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubmissionDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Submitted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

        // 🔹 Get by User
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _service.GetByUserAsync(userId);

            return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
        }

        // 🔹 Get by WorkItem
        [HttpGet("workitem/{workItemId}")]
        public async Task<IActionResult> GetByWorkItem(int workItemId)
        {
            var result = await _service.GetByWorkItemAsync(workItemId);

            return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
        }
    }
}