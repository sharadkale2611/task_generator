namespace task_generator.Dto
{
    public class CreateRoleDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!; // ADMIN, INTERN, MENTOR
    }
}