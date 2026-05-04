using task_generator.Dto;

namespace task_generator.Services
{
	public interface IProjectCategoryService
	{
		Task<IEnumerable<ProjectCategoryResponseDto>> GetAllAsync(bool? isActive);
		Task<ProjectCategoryResponseDto> CreateAsync(ProjectCategoryCreateDto dto);
		Task<ProjectCategoryResponseDto> UpdateAsync(int id, ProjectCategoryUpdateDto dto);
		Task<ProjectCategoryResponseDto?> GetByIdAsync(int id);
	}
}