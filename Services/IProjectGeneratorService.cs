using task_generator.Dto;
using task_generator.Helpers;

namespace task_generator.Services
{
	public interface IProjectGeneratorService
	{
		Task<ProjectGeneratorResult> GenerateAsync(EpicGeneratorRequest request);
		Task<string> GeneratePromt(EpicGeneratorRequest request);
	}
}
