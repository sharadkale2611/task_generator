namespace task_generator.Models
{
	public class ProjectCategory
	{
		public int ProjectCategoryId { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsActive { get; set; }
		public ICollection<Epic> Epics { get; set; } = new List<Epic>();
	}
}
