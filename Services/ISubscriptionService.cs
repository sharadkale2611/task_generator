using task_generator.Models;

namespace task_generator.Services
{
    public interface ISubscriptionService
    {
        Task<List<SubscriptionPlan>> GetAllPlansAsync();

        Task<SubscriptionPlan> CreatePlanAsync(SubscriptionPlan plan);

        Task<TenantSubscription> AssignPlanToTenantAsync(int tenantId, int planId);

        Task<TenantSubscription> ChangePlanAsync(int tenantId, int newPlanId);

        Task<TenantSubscription?> GetActiveSubscriptionAsync(int tenantId);
    }   
}
