using Microsoft.AspNetCore.Mvc;
using task_generator.AI;
using task_generator.Dto;
using task_generator.Helpers;
using task_generator.Services;

namespace task_generator.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EpicController : ControllerBase
	{
		private readonly IEpicCreateResponse _epicCreator;
		private readonly ITechStackService _techStackService;
		private readonly IProjectCategoryService _categoryService;
		private readonly IProjectDomainService _domainService;

		private readonly IEpicService _epicService;

		public EpicController(
			IEpicCreateResponse epicCreator,
			ITechStackService techStackService,
			IProjectCategoryService categoryService,
			IProjectDomainService domainService,
			IEpicService epicService)
		{
			_epicCreator = epicCreator;
			_techStackService = techStackService;
			_categoryService = categoryService;
			_domainService = domainService;
			_epicService = epicService;
		}

			[HttpPost("generate-epics")]
			public async Task<IActionResult> GenerateEpics([FromBody] EpicGeneratorRequest request)
			{
				if (request == null)
					return BadRequest("Invalid request");

				// ✅ STEP 1: Fetch data from DB
				var techStacks = await _techStackService.GetByIdsAsync(request.TechStackIds);
				var category = await _categoryService.GetByIdAsync(request.ProjectCategoryId);
				var domain = await _domainService.GetByIdAsync(request.ProjectDomainId);

				if (category == null || domain == null)
					return BadRequest("Invalid category or domain");

				// ✅ STEP 2: Convert to AI-friendly format (Names)
				var techStackNames = string.Join(", ", techStacks.Select(x => $"{x.Name} ({x.Category})"));
				var categoryName = category.Name;
				var domainName = domain.Name;

				var previousProjects = await _epicService
					.GetProjectNamesAsync(request.ProjectCategoryId, request.ProjectDomainId);

				// ✅ STEP 3: Build AI Context
				var aiContext = new EpicAIContext
				{
					TechStack = techStackNames,
					ProjectCategory = categoryName,
					Domain = domainName,
					Level = request.Level.ToString(),

					// Optional fields (extend later if needed)
					Goal = null,
					IndustryFocus = null,
					PreviousProjects = previousProjects
				};

				// ✅ STEP 4: Build Prompt
				var prompt = EpicPromptBuilder.Build(aiContext);

				// ✅ STEP 5: Call AI Service
				var result = await _epicCreator.Generate(prompt);

				// ✅ STEP 6: Return AI Response (for now)
				return Ok(result);
			}

		[HttpPost("create")]
		public async Task<IActionResult> CreateEpic([FromBody] CreateEpicDto dto)
		{
			if (dto == null)
				return BadRequest(HttpResponseDto<object>.FailureResponse("Invalid request"));

			// 🔥 BASIC VALIDATION
			if (string.IsNullOrWhiteSpace(dto.ProjectName))
				return BadRequest(HttpResponseDto<object>.FailureResponse("Project name is required"));

			if (dto.ProjectCategoryId <= 0 || dto.ProjectDomainId <= 0)
				return BadRequest(HttpResponseDto<object>.FailureResponse("Category and Domain are required"));

			if (dto.TechStackIds == null || !dto.TechStackIds.Any())
				return BadRequest(HttpResponseDto<object>.FailureResponse("At least one tech stack is required"));

			// 🔥 UNIQUE CHECK
			bool exists = await _epicService.ExistsAsync(
				dto.ProjectName,
				dto.ProjectCategoryId,
				dto.ProjectDomainId
			);

			if (exists)
			{
				return Conflict(HttpResponseDto<object>.FailureResponse(
					"An epic with same Project Name, Category and Domain already exists"
				));
			}

			// 🔥 CREATE
			var result = await _epicService.CreateAsync(dto);

			return Ok(HttpResponseDto<object>.SuccessResponse(result, "Epic created successfully"));
		}


		[HttpGet("list")]
		public async Task<IActionResult> GetEpics(
			[FromQuery] string? search,
			[FromQuery] string? status = "All")
		{
			var data = await _epicService.GetAllAsync(search, status);

			return Ok(HttpResponseDto<object>.SuccessResponse(data, "Fetched successfully"));
		}


	}
}