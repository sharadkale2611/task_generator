namespace task_generator.Models
{
	public class EpicTechStack
	{
		public int EpicId { get; set; }
		public Epic Epic { get; set; } = null!;

		public int TechStackId { get; set; }
		public TechStack TechStack { get; set; } = null!;
	}
}
	