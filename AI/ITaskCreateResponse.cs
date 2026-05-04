namespace task_generator.AI
{
	public interface ITaskCreateResponse
	{
		Task<string> Generate(string prompt);
	}
}
