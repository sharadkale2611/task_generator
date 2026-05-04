using task_generator.Dto;

namespace task_generator.Services
{
    public interface ISubmissionService
    {
        Task<SubmissionResponseDto> CreateAsync(CreateSubmissionDto dto);
        Task<List<SubmissionResponseDto>> GetByUserAsync(int userId);
        Task<List<SubmissionResponseDto>> GetByWorkItemAsync(int workItemId);
    }
}