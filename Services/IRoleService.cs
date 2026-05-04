using task_generator.Dto;

namespace task_generator.Services
{
    public interface IRoleService
    {
        Task<RoleResponseDto> CreateAsync(CreateRoleDto dto);
        Task<List<RoleResponseDto>> GetAllAsync();
        Task<RoleResponseDto?> GetByIdAsync(int roleId);
        Task<RoleResponseDto> UpdateAsync(int roleId, UpdateRoleDto dto);
        Task<bool> DeleteAsync(int roleId);
    }
}