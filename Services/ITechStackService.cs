using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
	public interface ITechStackService
	{
		Task<IEnumerable<TechStackResponseDto>> GetAllAsync(bool? isActive);
		Task<TechStackResponseDto> CreateAsync(TechStackCreateDto dto);
		Task<TechStackResponseDto> UpdateAsync(int id, TechStackUpdateDto dto);
		Task<List<TechStackResponseDto>> GetByIdsAsync(List<int> ids);
	}
}