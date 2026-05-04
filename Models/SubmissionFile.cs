namespace task_generator.Models
{
    public class SubmissionFile
    {
        public int SubmissionFileId { get; set; }

        public int SubmissionId { get; set; }

        public string FilePath { get; set; } = "";
        public string Status { get; set; } = ""; // added, modified, removed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
