using task_generator.AI;
using task_generator.Dto;
using task_generator.Helpers;
using System.Text.Json;

namespace task_generator.Services
{
	public class EpicGeneratorService : IEpicGeneratorService
	{
		private readonly IEpicCreateResponse _epicCreator;
		private readonly ITechStackService _techStackService;
		private readonly IProjectCategoryService _categoryService;
		private readonly IProjectDomainService _domainService;

		public EpicGeneratorService(
			IEpicCreateResponse epicCreator,
			ITechStackService techStackService,
			IProjectCategoryService categoryService,
			IProjectDomainService domainService)
		{
			_epicCreator = epicCreator;
			_techStackService = techStackService;
			_categoryService = categoryService;
			_domainService = domainService;
		}

		public async Task<EpicGeneratorResponse> GenerateAsync(EpicGeneratorRequest request)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			// ✅ STEP 1: Fetch data from DB
			var techStacks = await _techStackService.GetByIdsAsync(request.TechStackIds);
			var category = await _categoryService.GetByIdAsync(request.ProjectCategoryId);
			var domain = await _domainService.GetByIdAsync(request.ProjectDomainId);

			if (category == null || domain == null)
				throw new Exception("Invalid category or domain");

			// ✅ STEP 2: Convert to AI-friendly format
			var techStackNames = string.Join(", ", techStacks.Select(x => $"{x.Name} ({x.Category})"));

			var aiContext = new EpicAIContext
			{
				TechStack = techStackNames,
				ProjectCategory = category.Name,
				Domain = domain.Name,
				Level = request.Level.ToString(),

				// Optional (can extend later)
				Goal = null,
				IndustryFocus = null,
				PreviousProjects = null
			};

			// ✅ STEP 3: Build Prompt
			var prompt = EpicPromptBuilder.Build(aiContext);

			// ✅ STEP 4: Call AI
			var aiResponse = await _epicCreator.Generate(prompt);

			var result = JsonSerializer.Deserialize<EpicGeneratorResponse>(
				aiResponse,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);

			if (result == null)
				throw new Exception("Failed to parse AI response");


			// ✅ STEP 5: Return result
			return result;
		}

		public async Task<string> GeneratePromptAsync(EpicGeneratorRequest request)
		{
			// Optional helper (for debugging/testing prompt)

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