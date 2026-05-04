namespace task_generator.Models
{
	public class Sprint
	{
		public int SprintId { get; set; }
		public int EpicId { get; set; }

		public string Name { get; set; } = string.Empty;

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public string Status { get; set; } = "Planned";

        public int Order { get; set; }

        public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
	}
}
