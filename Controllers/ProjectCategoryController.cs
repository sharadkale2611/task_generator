using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
	[Route("api/project-categories")]
	[ApiController]
	public class ProjectCategoryController : ControllerBase
	{
		private readonly IProjectCategoryService _service;

		public ProjectCategoryController(IProjectCategoryService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] bool? isActive)
		{
			var data = await _service.GetAllAsync(isActive);

			return Ok(HttpResponseDto<IEnumerable<ProjectCategoryResponseDto>>
				.SuccessResponse(data, "Fetched successfully"));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ProjectCategoryCreateDto dto)
		{
			var data = await _service.CreateAsync(dto);

			return Ok(HttpResponseDto<ProjectCategoryResponseDto>
				.SuccessResponse(data, "Project category created successfully"));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] ProjectCategoryUpdateDto dto)
		{
			var data = await _service.UpdateAsync(id, dto);

			return Ok(HttpResponseDto<ProjectCategoryResponseDto>
				.SuccessResponse(data, "Project category updated successfully"));
		}
	}
}