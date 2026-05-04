using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
    public interface ITenantService
    {
        Task<Tenant> CreateTenantAsync(CreateTenantDto dto);
        Task<List<TenantListDto>> GetTenantsAsync();
        Task<TenantDetailDto?> GetTenantByIdAsync(int tenantId);


        Task<Tenant> UpdateAsync(int tenantId, TenantUpdateDto dto);
        Task<bool> DeleteAsync(int tenantId);

    }
}
