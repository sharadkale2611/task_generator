using System.ComponentModel.DataAnnotations;

namespace task_generator.Dto
{
	public class ProjectCategoryCreateDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;
	}

	public class ProjectCategoryUpdateDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public bool IsActive { get; set; }
	}

	public class ProjectCategoryResponseDto
	{
		public int ProjectCategoryId { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsActive { get; set; }
	}
}
