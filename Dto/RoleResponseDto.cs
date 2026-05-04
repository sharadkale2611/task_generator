namespace task_generator.Dto
{
    public class RoleResponseDto
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}