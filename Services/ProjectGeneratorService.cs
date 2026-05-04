using System.Text.Json;
using task_generator.AI;
using task_generator.Dto;
using task_generator.Helpers;
using task_generator.Services;

namespace task_generator.Services
{
	public class ProjectGeneratorService : IProjectGeneratorService
	{
		private readonly IAiClient _aiClient;
		private readonly ITechStackService _techStackService;
		private readonly IProjectCategoryService _categoryService;
		private readonly IProjectDomainService _domainService;

		public ProjectGeneratorService(
			IAiClient aiClient,
			ITechStackService techStackService,
			IProjectCategoryService categoryService,
			IProjectDomainService domainService)
		{
			_aiClient = aiClient;
			_techStackService = techStackService;
			_categoryService = categoryService;
			_domainService = domainService;
		}

		public async Task<ProjectGeneratorResult> GenerateAsync(EpicGeneratorRequest request)
		{
			// ✅ STEP 1: Fetch DB data
			var techStacks = await _techStackService.GetByIdsAsync(request.TechStackIds);
			var category = await _categoryService.GetByIdAsync(request.ProjectCategoryId);
			var domain = await _domainService.GetByIdAsync(request.ProjectDomainId);

			if (category == null || domain == null)
				throw new ArgumentException("Invalid category or domain");

			// ✅ STEP 2: Build AI Context
			var techStackNames = string.Join(", ", techStacks.Select(x => $"{x.Name} ({x.Category})"));

			var aiContext = new EpicAIContext
			{
				TechStack = techStackNames,
				ProjectCategory = category.Name,
				Domain = domain.Name,
				Level = request.Level.ToString()
			};

			// ✅ STEP 3: Build Prompt
			var prompt = EpicPromptBuilder.Build(aiContext);

			// ✅ STEP 4: Call AI
			var aiResponse = await _aiClient.GetCompletionAsync(prompt);

			// ✅ STEP 5: Deserialize JSON → DTO
			var result = JsonSerializer.Deserialize<ProjectGeneratorResult>(
				aiResponse,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);

			if (result == null)
				throw new Exception("AI response parsing failed");

			return result;
		}

		// ✅ FIXED method signature
		public async Task<string> GeneratePromt(EpicGeneratorRequest request)
		{
			var techStacks = await _techStackService.GetByIdsAsync(request.TechStackIds);
			var category = await _categoryService.GetByIdAsync(request.ProjectCategoryId);
			var domain = await _domainService.GetByIdAsync(request.ProjectDomainId);

			var techStackNames = string.Join(", ", techStacks.Select(x => $"{x.Name} ({x.Category})"));

			var aiContext = new EpicAIContext
			{
				TechStack = techStackNames,
				ProjectCategory = category?.Name ?? "",
				Domain = domain?.Name ?? "",
				Level = request.Level.ToString()
			};

			return EpicPromptBuilder.Build(aiContext);
		}
	}
}