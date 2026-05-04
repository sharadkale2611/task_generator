using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // 🔹 Create Role --
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            try
            {
                var result = await _roleService.CreateAsync(dto);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Role created"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get All Roles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _roleService.GetAllAsync();

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Get Role by Id
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetById(int roleId)
        {
            try
            {
                var result = await _roleService.GetByIdAsync(roleId);

                if (result == null)
                {
                    return Ok(HttpResponseDto<object>.SuccessResponse(null, "Role not found"));
                }

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Fetched"));
            }
            catch (Exception ex)
            {
                return NotFound(HttpResponseDto<object>.FailureResponse(ex.Message));
            }
        }

        // 🔹 Update Role
        [HttpPut("{roleId}")]
        public async Task<IActionResult> Update(int roleId, [FromBody] UpdateRoleDto dto)
        {
            try
            {
                var result = await _roleService.UpdateAsync(roleId, dto);

                return Ok(HttpResponseDto<object>.SuccessResponse(result, "Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // 🔹 Delete Role (Deactivate)
        [HttpDelete("{roleId}")]
        public async Task<IActionResult> Delete(int roleId)
        {
            try
            {
                await _roleService.DeleteAsync(roleId);

                return Ok(HttpResponseDto<object>.SuccessResponse(null, "Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}