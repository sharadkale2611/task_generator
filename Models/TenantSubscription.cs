using task_generator.Helpers;

namespace task_generator.Models
{
    public class TenantSubscription
    {
        public int TenantSubscriptionId { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        // ✅ FK to SubscriptionPlan table
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsTrial { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
