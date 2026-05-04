namespace task_generator.Dto
{
    public class AssignWorkItemDto
    {
        public int WorkItemId { get; set; }
        public int UserId { get; set; }
        public int AssignedByUserId { get; set; }
        public string? Reason { get; set; }
    }
}