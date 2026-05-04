using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkItemAssignmentController : ControllerBase
    {
        private readonly IWorkItemAssignmentService _service;

        public WorkItemAssignmentController(IWorkItemAssignmentService service)
        {
            _service = service;
        }

        // 🔹 Assign / Reassign
        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] AssignWorkItemDto dto)
        {
            try
            {
                await _service.AssignAsync(dto);

                return Ok(HttpResponseDto<object>.SuccessResponse(null, "Assigned successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

        // 🔹 Get Assignment History
        [HttpGet("history/{workItemId}")]
        public async Task<IActionResult> GetHistory(int workItemId)
        {
            try
            {
                var result = await _service.GetHistoryAsync(workItemId);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

        // 🔹 Get Active Assignment
        [HttpGet("active/{workItemId}")]
        public async Task<IActionResult> GetActive(int workItemId)
        {
            try
            {
                var result = await _service.GetActiveAssignmentAsync(workItemId);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }
    }
}