using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Models;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
            {
                _subscriptionService = subscriptionService;
            }

        // 🔹 Get all active plans
        [HttpGet("plans")]
        public async Task<IActionResult> GetAllPlans()
        {
            try
            {
                var result = await _subscriptionService.GetAllPlansAsync();

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Plans fetched"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Create new plan (Admin)
        [HttpPost("plans")]
        public async Task<IActionResult> CreatePlan([FromBody] SubscriptionPlan plan)
        {
            try
            {
                var result = await _subscriptionService.CreatePlanAsync(plan);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Plan created"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Assign plan to tenant (first time)
        [HttpPost("assign")]
        public async Task<IActionResult> AssignPlan([FromQuery] int tenantId, [FromQuery] int planId)
        {
            try
            {
                var result = await _subscriptionService.AssignPlanToTenantAsync(tenantId, planId);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Plan assigned"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔥 Change / Upgrade plan
        [HttpPut("change")]
        public async Task<IActionResult> ChangePlan([FromQuery] int tenantId, [FromQuery] int newPlanId)
        {
            try
            {
                var result = await _subscriptionService.ChangePlanAsync(tenantId, newPlanId);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Plan updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get active subscription of tenant
        [HttpGet("active/{tenantId}")]
        public async Task<IActionResult> GetActiveSubscription(int tenantId)
        {
            try
            {
                var result = await _subscriptionService.GetActiveSubscriptionAsync(tenantId);

                if (result == null)
                {
                    return Ok(HttpResponseDto<object>.SuccessResponse(
                        result,
                        "No active subscription found"
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
