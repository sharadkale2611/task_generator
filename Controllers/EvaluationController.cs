using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        // 🔹 Run evaluation (AI) 
        [HttpPost("{submissionId}")]
        public async Task<IActionResult> Evaluate(int submissionId)
        {
            try
            {
                var result = await _evaluationService.EvaluateAsync(submissionId);

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    result,
                    "Evaluation completed successfully"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

        // 🔹 Get evaluation result
        [HttpGet("{submissionId}")]
        public async Task<IActionResult> Get(int submissionId)
        {
            try
            {
                var result = await _evaluationService.GetBySubmissionAsync(submissionId);

                if (result == null)
                {
                    return Ok(HttpResponseDto<object>.SuccessResponse(
                        null,
                        "No evaluation found"
                    ));
                }

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    result,
                    "Fetched"
                ));
            }
            catch (Exception ex)
            {
                return NotFound(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }
    }
}