namespace task_generator.Dto
{
    public class SubmissionResponseDto
    {
        public int SubmissionId { get; set; }

        public int WorkItemId { get; set; }
        public string WorkItemTitle { get; set; } = null!;

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;

        public string? SubmissionNotes { get; set; }

        public string? RepoUrl { get; set; }
        public string? BranchName { get; set; }
        public string? BaseBranch { get; set; }

        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}