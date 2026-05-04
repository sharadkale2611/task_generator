namespace task_generator.AI.Fake
{
	public class FakeSprintCreateResponse : ISprintCreateResponse
	{
		public Task<string> Generate(string prompt)
		{
			// TEMP: Hardcoded response (for debugging Sprint Builder)
			return Task.FromResult("""
[
  {
    "sprintNumber": 1,
    "moduleName": "Project Setup & Database Design",
    "description": "Initialize project structure, setup database schema, and configure environment.",
    "estimatedDays": 2
  },
  {
    "sprintNumber": 2,
    "moduleName": "Authentication & Authorization",
    "description": "Implement user registration, login, JWT authentication, and role-based access.",
    "estimatedDays": 3
  },
  {
    "sprintNumber": 3,
    "moduleName": "Core Business Logic (CRUD Operations)",
    "description": "Develop APIs and UI for managing main entities with full CRUD functionality.",
    "estimatedDays": 4
  },
  {
    "sprintNumber": 4,
    "moduleName": "UI Integration & Testing",
    "description": "Integrate frontend with backend APIs, perform validation, and test all features.",
    "estimatedDays": 3
  }
]
""");
		}
	}
}