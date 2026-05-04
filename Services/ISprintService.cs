using task_generator.Models;

namespace task_generator.Services
{
    public interface ISprintService
    {
        Task<List<Sprint>> GenerateAsync(int epicId);
        Task<List<Sprint>> GetByEpicIdAsync(int epicId);
    }
}
