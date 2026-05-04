namespace task_generator.AI
{
	public interface IEpicCreateResponse
	{
		Task<string> Generate(string prompt);

	}
}
