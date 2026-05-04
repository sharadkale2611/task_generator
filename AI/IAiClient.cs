namespace task_generator.AI
{
	public interface IAiClient
	{
		Task<string> GetCompletionAsync(string prompt);
	}
}
