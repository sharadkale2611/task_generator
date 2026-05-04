namespace task_generator.Dto
{
    public class WorkItemAssignmentResponseDto
    {
        public int WorkItemAssignmentId { get; set; }

        public int WorkItemId { get; set; }
        public string WorkItemTitle { get; set; } = null!;

        public int AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; } = null!;

        public int? AssignedByUserId { get; set; }

        public string? Reason { get; set; }

        public DateTime AssignedAt { get; set; }
        public DateTime? UnassignedAt { get; set; }

        public bool IsActive { get; set; }
    }
}