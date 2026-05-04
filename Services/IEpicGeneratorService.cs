using task_generator.Dto;

namespace task_generator.Services
{
	public interface IEpicGeneratorService
	{
		Task<EpicGeneratorResponse> GenerateAsync(EpicGeneratorRequest request);
		Task<string> GeneratePromptAsync(EpicGeneratorRequest request);

	}
}
