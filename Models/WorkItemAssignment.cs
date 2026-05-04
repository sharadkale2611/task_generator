namespace task_generator.Models
{
    public class WorkItemAssignment
    {
        public int WorkItemAssignmentId { get; set; }

        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; } = null!;

        public int TenantId { get; set; }

        public int AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; } = null!;

        public int? AssignedByUserId { get; set; } // admin


        public string? Reason { get; set; } // 🔥 WHY reassigned

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UnassignedAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}