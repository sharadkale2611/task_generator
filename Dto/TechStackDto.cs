using System.ComponentModel.DataAnnotations;

namespace task_generator.Dto
{
	public class TechStackCreateDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Category { get; set; } = string.Empty;  // Frontend, Backend, DB, AI, Other
	}

	public class TechStackUpdateDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Category { get; set; } = string.Empty;  // Frontend, Backend, DB, AI, Other

		[Required]
		public bool IsActive { get; set; }
	}

	public class TechStackResponseDto
	{
		public int TechStackId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;    // Frontend, Backend, DB, AI, Other
		public bool IsActive { get; set; }
	}
}
