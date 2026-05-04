using task_generator.Models;

namespace task_generator.Services
{
	public interface IWorkItemService
	{
		Task<List<WorkItem>> GenerateAsync(int sprintId);
		Task<List<WorkItem>> GetBySprintIdAsync(int sprintId);
        Task<bool> UpdateStatusAsync(int workItemId, string status);
    }
}