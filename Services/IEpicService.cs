using task_generator.Dto;

namespace task_generator.Services
{
	public interface IEpicService
	{
		Task<List<string>> GetProjectNamesAsync(int categoryId, int domainId);

		Task<EpicResponseDto> CreateAsync(CreateEpicDto dto);
		Task<EpicResponseDto> UpdateAsync(UpdateEpicDto dto);
		Task<bool> SoftDeleteAsync(int epicId);

		Task<bool> ExistsAsync(string projectName, int categoryId, int domainId);

		Task<List<EpicResponseDto>> GetAllAsync(string? search, string? status);
	}
}
