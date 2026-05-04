using task_generator.Dto;

namespace task_generator.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateAsync(CreateUserDto dto);
        Task<List<UserResponseDto>> GetByTenantAsync(int tenantId);
        Task<UserResponseDto?> GetByIdAsync(int userId);
        Task<UserResponseDto> UpdateAsync(int userId, UpdateUserDto dto);
        Task<bool> DeleteAsync(int userId);
    }
}