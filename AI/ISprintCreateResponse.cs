namespace task_generator.AI
{
	public interface ISprintCreateResponse
	{
		Task<string> Generate(string prompt);

	}
}
