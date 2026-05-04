namespace task_generator.Models
{
	public class Epic
	{
		public int EpicId { get; set; }
		public string ProjectName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty ;

		// Foreign Keys
		public int ProjectCategoryId { get; set; }
		public int ProjectDomainId { get; set; }
		public string Status { get; set; } = string.Empty;
		public string Level { get; set; } = "Beginner"; // Beginner, Intermediate, Advanced
		public string EstimatedDuration { get; set; } = string.Empty; // e.g. "2 weeks"
		public string Source { get; set; } = "AI"; // AI / Manual
		public bool IsApproved { get; set; } = false;

		// Navigation Properties
		public ProjectCategory ProjectCategory { get; set; } = null!;
		public ProjectDomain ProjectDomain { get; set; } = null!;

		// Many-to-Many with TechStack
		public ICollection<EpicTechStack> EpicTechStacks { get; set; } = new List<EpicTechStack>();
	}
}
