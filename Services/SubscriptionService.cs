using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Models;

namespace task_generator.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _db;

        public SubscriptionService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 🔹 Get all active plans
        public async Task<List<SubscriptionPlan>> GetAllPlansAsync()
        {
            return await _db.SubscriptionPlans
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();
        }

        // 🔹 Create new plan (Admin only)
        public async Task<SubscriptionPlan> CreatePlanAsync(SubscriptionPlan plan)
        {
            try
            {

                await _db.SubscriptionPlans.AddAsync(plan);
                await _db.SaveChangesAsync();
                return plan;
            }
            catch (Exception e)
            {
                // Log the exception (e.g., using a logging framework)
                Console.WriteLine($"Error creating plan: {e.Message}");
                throw new Exception("An error occurred while creating the subscription plan.");
            }
        }

        // 🔹 Assign plan to tenant (First time)
        public async Task<TenantSubscription> AssignPlanToTenantAsync(int tenantId, int planId)
        {
            var plan = await _db.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.SubscriptionPlanId == planId && p.IsActive);

            if (plan == null)
                throw new Exception("Invalid plan");

            var existing = await _db.TenantSubscriptions
                .AnyAsync(s => s.TenantId == tenantId && s.IsActive);

            if (existing)
                throw new Exception("Tenant already has active subscription");

            var subscription = new TenantSubscription
            {
                TenantId = tenantId,
                SubscriptionPlanId = planId,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                IsTrial = true
            };

            await _db.TenantSubscriptions.AddAsync(subscription);
            await _db.SaveChangesAsync();

            return subscription;
        }

        // 🔥 Change / Upgrade plan
        public async Task<TenantSubscription> ChangePlanAsync(int tenantId, int newPlanId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var current = await _db.TenantSubscriptions
                    .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.IsActive);

                if (current == null)
                    throw new Exception("No active subscription found");

                var newPlan = await _db.SubscriptionPlans
                    .FirstOrDefaultAsync(p => p.SubscriptionPlanId == newPlanId && p.IsActive);

                if (newPlan == null)
                    throw new Exception("Invalid plan");

                // 🔴 Deactivate old
                current.IsActive = false;
                current.EndDate = DateTime.UtcNow;

                // 🟢 Create new
                var newSubscription = new TenantSubscription
                {
                    TenantId = tenantId,
                    SubscriptionPlanId = newPlanId,
                    StartDate = DateTime.UtcNow,
                    IsActive = true,
                    IsTrial = false
                };

                await _db.TenantSubscriptions.AddAsync(newSubscription);

                // 🔥 Update usage limits
                var usage = await _db.TenantUsages
                    .FirstOrDefaultAsync(u => u.TenantId == tenantId);

                if (usage != null)
                {
                    usage.AiCreditsLimit = newPlan.AiCreditsLimit;
                    usage.LastUpdated = DateTime.UtcNow;
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return newSubscription;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // 🔹 Get active subscription
        public async Task<TenantSubscription?> GetActiveSubscriptionAsync(int tenantId)
        {
            return await _db.TenantSubscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.IsActive);
        }
    }
}