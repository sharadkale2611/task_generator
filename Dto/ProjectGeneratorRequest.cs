namespace task_generator.Dto
{
	public class ProjectGeneratorRequest
	{
		public string TechStack { get; set; }   // ".NET + MSSQL + React"
		public string Level { get; set; }       // Beginner
		public int DurationDays { get; set; }   // 7 or 10
		public string ProjectType { get; set; } // CRUD
		public string? Domain { get; set; }   // NEW
		public List<string>? PreviousProjects { get; set; }
		public bool IsPreview { get; set; }

	}
}
