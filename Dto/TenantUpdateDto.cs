namespace task_generator.Dto
{
    public class TenantUpdateDto
    {
        // 🔹 Basic Info
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        // 🔹 Status
        public bool IsActive { get; set; }

        // 🔹 Address (Primary)
        public string AddressLine1 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}