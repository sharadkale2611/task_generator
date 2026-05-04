namespace task_generator.Models
{
    public class TenantUsage
    {
        public int TenantUsageId { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }

        public decimal AiCreditsUsed { get; set; }
        public decimal AiCreditsLimit { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
