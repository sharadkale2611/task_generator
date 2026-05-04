using task_generator.Dto;

namespace task_generator.Services
{
    public interface IEvaluationService
    {
        Task<EvaluationResponseDto> EvaluateAsync(int submissionId);
        Task<EvaluationResponseDto?> GetBySubmissionAsync(int submissionId);
    }
}