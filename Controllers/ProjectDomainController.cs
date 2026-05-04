using Microsoft.AspNetCore.Mvc;
using task_generator.Dto;
using task_generator.Services;

namespace task_generator.Controllers
{
	[Route("api/project-domains")]
	[ApiController]
	public class ProjectDomainController : ControllerBase
	{
		private readonly IProjectDomainService _service;

		public ProjectDomainController(IProjectDomainService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> Get([FromQuery] bool? isActive)
		{
			var data = await _service.GetAllAsync(isActive);

			return Ok(HttpResponseDto<IEnumerable<ProjectDomainResponseDto>>
				.SuccessResponse(data, "Fetched successfully"));
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] ProjectDomainCreateDto dto)
		{
			var data = await _service.CreateAsync(dto);

			return Ok(HttpResponseDto<ProjectDomainResponseDto>
				.SuccessResponse(data, "Project domain created successfully"));
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] ProjectDomainUpdateDto dto)
		{
			var data = await _service.UpdateAsync(id, dto);

			return Ok(HttpResponseDto<ProjectDomainResponseDto>
				.SuccessResponse(data, "Project domain updated successfully"));
		}
	}
}