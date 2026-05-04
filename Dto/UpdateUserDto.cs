namespace task_generator.Dto
{
    public class UpdateUserDto
    {
        public int RoleId { get; set; }

        public string Name { get; set; } = null!;
        public string? Phone { get; set; }

        public bool IsActive { get; set; }
    }
}