using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 🔹 Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                var result = await _userService.CreateAsync(dto);
                return Ok(HttpResponseDto<object>.SuccessResponse(result, "User created"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get by Tenant
        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult> GetByTenant(int tenantId)
        {
            try
            {
                var result = await _userService.GetByTenantAsync(tenantId);
                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get by Id
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            try
            {
                var result = await _userService.GetByIdAsync(userId);

                if (result == null)
                    return Ok(HttpResponseDto<object>.SuccessResponse(null, "User not found"));

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Update
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(int userId, [FromBody] UpdateUserDto dto)
        {
            try
            {
                var result = await _userService.UpdateAsync(userId, dto);
                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Delete
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            try
            {
                await _userService.DeleteAsync(userId);
                return Ok(HttpResponseDto<object>.SuccessResponse(null, "Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}