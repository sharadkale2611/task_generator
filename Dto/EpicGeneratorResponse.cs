namespace task_generator.Dto
{

	public class EpicDto
	{
		public string ProjectName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public string ProjectCategory { get; set; } = string.Empty;
		public string Domain { get; set; } = string.Empty;

		public string Level { get; set; } = string.Empty;
		public string EstimatedDuration { get; set; } = string.Empty;

		public List<string> TechStack { get; set; } = new();

		public string Status { get; set; } = "Generated";
		public string Source { get; set; } = "AI";
	}

	public class EpicGeneratorResponse
	{
		public List<EpicDto> Epics { get; set; } = new();
	}

	public class CreateEpicDto
	{
		public string ProjectName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public int ProjectCategoryId { get; set; }
		public int ProjectDomainId { get; set; }

		public EpicLevel Level { get; set; }

		public string EstimatedDuration { get; set; } = string.Empty;
		public string Source { get; set; } = "AI";

		public List<int> TechStackIds { get; set; } = new();
	}

	public class UpdateEpicDto
	{
		public int EpicId { get; set; }

		public string ProjectName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public int ProjectCategoryId { get; set; }
		public int ProjectDomainId { get; set; }

		public EpicLevel Level { get; set; }

		public string EstimatedDuration { get; set; } = string.Empty;
		public bool IsApproved { get; set; }

		public List<int> TechStackIds { get; set; } = new();
	}

	public class EpicResponseDto
	{
		public int EpicId { get; set; }
		public string ProjectName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public string ProjectCategory { get; set; } = string.Empty;
		public string Domain { get; set; } = string.Empty;

		public string Level { get; set; } = string.Empty;
		public string EstimatedDuration { get; set; } = string.Empty;

		public string Status { get; set; } = string.Empty;
		public string Source { get; set; } = string.Empty;

		public bool IsApproved { get; set; }

		public List<string> TechStacks { get; set; } = new();
	}

}
