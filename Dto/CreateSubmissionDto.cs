namespace task_generator.Dto
{
    public class CreateSubmissionDto
    {
        public int WorkItemId { get; set; }
        public int UserId { get; set; }

        public string? SubmissionNotes { get; set; }

        public string? RepoUrl { get; set; }
        public string? BranchName { get; set; }
        public string? BaseBranch { get; set; }
    }
}