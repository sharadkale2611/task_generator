namespace task_generator.Dto
{
    public class EvaluationResponseDto
    {
        public int EvaluationId { get; set; }

        public int SubmissionId { get; set; }

        public int Score { get; set; }
        public string Feedback { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}