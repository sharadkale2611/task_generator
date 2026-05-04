using task_generator.Dto;

namespace task_generator.Services
{
    public interface IWorkItemAssignmentService
    {
        Task<bool> AssignAsync(AssignWorkItemDto dto);
        Task<List<WorkItemAssignmentResponseDto>> GetHistoryAsync(int workItemId);
        Task<WorkItemAssignmentResponseDto?> GetActiveAssignmentAsync(int workItemId);
    }
}