using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class WorkItemController : ControllerBase
	{
		private readonly IWorkItemService _service;

		public WorkItemController(IWorkItemService service)
		{
			_service = service;
		}

		[HttpPost("generate")]
		public async Task<IActionResult> Generate([FromQuery] int sprintId)
		{
			var result = await _service.GenerateAsync(sprintId);

			return Ok(HttpResponseDto<object>.SuccessResponse(result, "Tasks generated"));
		}

		[HttpGet("sprint/{sprintId}")]
		public async Task<IActionResult> GetBySprint(int sprintId)
		{
			var result = await _service.GetBySprintIdAsync(sprintId);

			return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
		}

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateWorkItemStatusDto dto)
        {
            try
            {
                var updated = await _service.UpdateStatusAsync(dto.WorkItemId, dto.Status);

                if (!updated)
                    return NotFound(HttpResponseDto<object>.FailureResponse("WorkItem not found"));

                return Ok(HttpResponseDto<object>.SuccessResponse(null, "Status updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

    }
}
