namespace task_generator.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        // 🔹 Basic Info
        public string Name { get; set; } = null!;   // Admin, Intern, Mentor
        public string Code { get; set; } = null!;   // ADMIN, INTERN, MENTOR

        // 🔹 Control
        public bool IsActive { get; set; } = true;

        // 🔹 Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 🔹 Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}