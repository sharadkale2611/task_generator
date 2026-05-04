using task_generator.Helpers;

namespace task_generator.Models
{
    public class SubscriptionPlan
    {
        public int SubscriptionPlanId { get; set; } // PK

        // 🔹 Basic Info
        public string Name { get; set; } = null!; // Free, Basic, Pro
        public string? Description { get; set; }

        // 🔹 Pricing
        public decimal Price { get; set; }
        public BillingCycle BillingCycle { get; set; } // Monthly / Yearly

        // 🔹 Limits
        public int MaxStudents { get; set; }
        public int MaxAdmins { get; set; }

        // 🔹 AI Usage Control (VERY IMPORTANT)
        public decimal AiCreditsLimit { get; set; } // total credits per cycle

        // 🔹 Feature Flags
        public bool HasAiTaskGeneration { get; set; }
        public bool HasAiEvaluation { get; set; }
        public bool HasRecruiterAccess { get; set; }
        public bool HasAdvancedAnalytics { get; set; }
        public bool HasGithubIntegration { get; set; }

        // 🔹 Control
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } // for UI display

        // 🔹 Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
