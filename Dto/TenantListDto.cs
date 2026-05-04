namespace task_generator.Dto
{
    public class TenantListDto
    {
        public int TenantId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }

        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }

        public string PlanName { get; set; } = "N/A";
    }
}