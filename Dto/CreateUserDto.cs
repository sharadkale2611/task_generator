namespace task_generator.Dto
{
    public class CreateUserDto
    {
        public int TenantId { get; set; }
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        public string Password { get; set; } = null!;
    }
}