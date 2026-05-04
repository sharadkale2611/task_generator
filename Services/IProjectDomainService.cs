using task_generator.Dto;

namespace task_generator.Services
{
	public interface IProjectDomainService
	{
		Task<IEnumerable<ProjectDomainResponseDto>> GetAllAsync(bool? isActive);
		Task<ProjectDomainResponseDto> CreateAsync(ProjectDomainCreateDto dto);
		Task<ProjectDomainResponseDto> UpdateAsync(int id, ProjectDomainUpdateDto dto);
		Task<ProjectDomainResponseDto?> GetByIdAsync(int id);
	}
}