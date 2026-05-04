using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
	[Route("api/project-generator")]
	[ApiController]
	public class ProjectGeneratorController : ControllerBase
	{
		private readonly IProjectGeneratorService _projectGeneratorService;

		public ProjectGeneratorController(IProjectGeneratorService projectGeneratorService)
		{
			_projectGeneratorService = projectGeneratorService;
		}

		[HttpPost]
		public async Task<IActionResult> Generate(EpicGeneratorRequest request)
		{
			if (request == null)
				return BadRequest("Invalid request");

			var result = await _projectGeneratorService.GenerateAsync(request);

			return Ok(result);
		}

		[HttpPost("generate-prompt")]
		public IActionResult GeneratePrompt(EpicGeneratorRequest request)
		{
			if (request == null)
				return BadRequest("Invalid request");

			var result = _projectGeneratorService.GeneratePromt(request);

			return Ok(result);
		}


		
	}
}