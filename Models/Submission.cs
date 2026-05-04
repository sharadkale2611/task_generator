namespace task_generator.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }

        public int TenantId { get; set; }

        public int WorkItemId { get; set; }
        public WorkItem WorkItem { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string SubmissionNotes { get; set; } = ""; // github link / description
        public string? RepoUrl { get; set; }
        public string? BranchName { get; set; }
        public string? BaseBranch { get; set; } // e.g. main

        public string Status { get; set; } = "Submitted"; // Submitted, Reviewed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}