namespace task_generator.Models
{
    public class User
    {
        public int UserId { get; set; }

        // 🔹 Tenant Mapping (VERY IMPORTANT)
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        // 🔹 Role Mapping
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        // 🔹 Basic Info
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        // 🔹 Auth (simple for now)
        public string PasswordHash { get; set; } = null!;

        // 🔹 Status
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        // 🔹 Tracking
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = null;

        public DateTime? LastLoginAt { get; set; }

        // 🔹 Future Extensions
        public string? ProfileImageUrl { get; set; }

    }
}