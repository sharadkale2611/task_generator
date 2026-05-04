using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Models;
using task_generator.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace task_generator.Controllers
{
	[Route("api/tech-stacks")]
	[ApiController]
	public class TechStackController : ControllerBase
	{
		private readonly ITechStackService _techStack;

		public TechStackController(ITechStackService techStack)
		{
			_techStack = techStack;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] bool? isActive)
		{
			var data =  await _techStack.GetAllAsync(isActive);
			return Ok(HttpResponseDto<IEnumerable<TechStackResponseDto>>
				.SuccessResponse(data, "Fetched successfully"));

		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] TechStackCreateDto dto)
		{
			var data = await _techStack.CreateAsync(dto);

			return Ok(HttpResponseDto<TechStackResponseDto>
				.SuccessResponse(data, "Tech stack created successfully"));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] TechStackUpdateDto dto)
		{
			var data = await _techStack.UpdateAsync(id, dto);
			return Ok(HttpResponseDto<TechStackResponseDto>
				.SuccessResponse(data, "Tech stack created successfully"));

		}

	}
}
