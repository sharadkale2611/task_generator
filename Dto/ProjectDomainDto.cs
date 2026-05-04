using System.ComponentModel.DataAnnotations;

namespace task_generator.Dto
{
	public class ProjectDomainCreateDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;
	}

	public class ProjectDomainUpdateDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public bool IsActive { get; set; }
	}

	public class ProjectDomainResponseDto
	{
		public int ProjectDomainId { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsActive { get; set; }
	}
}
