namespace task_generator.Dto
{
    public class TenantDetailDto
    {
        public int TenantId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        public bool IsActive { get; set; }

        public string AddressLine1 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        public string PlanName { get; set; } = "N/A";
        public decimal AiCreditsUsed { get; set; }
        public decimal AiCreditsLimit { get; set; }

        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
    }
}