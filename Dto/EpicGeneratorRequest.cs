namespace task_generator.Dto
{
	public enum EpicLevel
	{
		Beginner,
		Intermediate,
		Expert,
	}
	public class EpicGeneratorRequest
	{
		// 🔗 Strong relational fields (FK style)
		public List<int> TechStackIds { get; set; } = new();
		public int ProjectCategoryId { get; set; }
		public int ProjectDomainId { get; set; }
		public EpicLevel Level { get; set; } = EpicLevel.Beginner;

	}


	public class EpicAIContext
	{
		public string TechStack { get; set; } = string.Empty;
		public string ProjectCategory { get; set; } = string.Empty;
		public string Domain { get; set; } = string.Empty;
		public string Level { get; set; } = string.Empty;

		public string? Goal { get; set; }
		public string? IndustryFocus { get; set; }

		public List<string>? PreviousProjects { get; set; }
	}

}
