using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        // 🔹 Create Tenant (Full onboarding)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantDto dto)
        {
            try
            {
                var result = await _tenantService.CreateTenantAsync(dto);

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    result,
                    "Tenant created successfully"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get all tenants (List view)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _tenantService.GetTenantsAsync();

                if (!result.Any())
                {
                    return Ok(HttpResponseDto<object>.SuccessResponse(
                        result,
                        "No tenants found"
                    ));
                }

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    result,
                    "Tenants fetched"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get tenant details
        [HttpGet("{tenantId}")]
        public async Task<IActionResult> GetById(int tenantId)
        {
            try
            {
                var result = await _tenantService.GetTenantByIdAsync(tenantId);

                if (result == null)
                {
                    return Ok(HttpResponseDto<object>.SuccessResponse(
                        result,
                        "Tenant not found"
                    ));
                }

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    result,
                    "Tenant fetched"
                ));
            }
            catch (Exception ex)
            {
                return NotFound(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

        // 🔹 Update Tenant
        [HttpPut("{tenantId}")]
        public async Task<IActionResult> Update(int tenantId, [FromBody] TenantUpdateDto dto)
        {
            try
            {
                var result = await _tenantService.UpdateAsync(tenantId, dto);

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    result,
                    "Tenant updated"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Delete Tenant (Soft delete)
        [HttpDelete("{tenantId}")]
        public async Task<IActionResult> Delete(int tenantId)
        {
            try
            {
                await _tenantService.DeleteAsync(tenantId);

                return Ok(HttpResponseDto<object>.SuccessResponse(
                    null,
                    "Tenant deleted"
                ));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}