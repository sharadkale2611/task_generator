namespace task_generator.AI.Fake
{
	public class FakeTaskCreateResponse : ITaskCreateResponse
	{
		public Task<string> Generate(string prompt)
		{
			// TEMP: Hardcoded response (for debugging Task Builder)
			return Task.FromResult("""
[
  {
    "title": "Authentication Module",
    "points": 30,
    "subTasks": [
      {
        "title": "Create login API",
        "points": 10
      },
      {
        "title": "Create registration API",
        "points": 10
      },
      {
        "title": "Implement JWT authentication",
        "points": 10
      }
    ]
  },
  {
    "title": "Student Management",
    "points": 40,
    "subTasks": [
      {
        "title": "Create student entity",
        "points": 10
      },
      {
        "title": "Implement CRUD APIs",
        "points": 15
      },
      {
        "title": "Create UI for student list",
        "points": 15
      }
    ]
  }
]
""");
		}
	}
}