namespace task_generator.Models
{
	public class TechStack
	{
		public int TechStackId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Category { get; set; } = string.Empty;	// Frontend, Backend, DB, AI, Other
		public bool IsActive { get; set; }

		public ICollection<EpicTechStack> EpicTechStacks { get; set; } = new List<EpicTechStack>();
	}
}
