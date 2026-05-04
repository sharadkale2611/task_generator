namespace task_generator.Dto
{
	public class TaskHierarchyDto
	{
		public string Title { get; set; } = string.Empty;
		public int Points { get; set; }
		public List<TaskHierarchyDto>? SubTasks { get; set; }
	}
}
