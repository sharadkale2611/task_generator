namespace task_generator.Dto
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; } = null!;

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        public bool IsActive { get; set; }
    }
}