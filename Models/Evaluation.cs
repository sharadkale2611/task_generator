namespace task_generator.Models
{
    public class Evaluation
    {
        public int EvaluationId { get; set; }

        public int SubmissionId { get; set; }
        public Submission Submission { get; set; } = null!;

        public int TenantId { get; set; }

        public int Score { get; set; } // 0–100
        public string Feedback { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}