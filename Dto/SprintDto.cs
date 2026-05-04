namespace task_generator.Dto
{
	public class SprintDto
	{
		public int SprintNumber { get; set; }
		public string ModuleName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int EstimatedDays { get; set; }
        public int Order { get; set; }
    }
}
