namespace task_generator.AI
{
    public class FakeAiClient : IAiClient
    {
        public Task<string> GetCompletionAsync(string prompt)
        {
            if (IsEvaluationPrompt(prompt))
            {
                return Task.FromResult(GetEvaluationResponse());
            }

            if (IsSprintPrompt(prompt))
            {
                return Task.FromResult(GetSprintResponse());
            }

            if (IsTaskPrompt(prompt))
            {
                return Task.FromResult(GetTaskResponse());
            }

            // fallback
            return Task.FromResult("[]");
        }

        private bool IsEvaluationPrompt(string prompt)
        {
            return prompt.Contains("Evaluate this task submission", StringComparison.OrdinalIgnoreCase)
                || prompt.Contains("score", StringComparison.OrdinalIgnoreCase)
                || prompt.Contains("feedback", StringComparison.OrdinalIgnoreCase);
        }

        // 🔍 Detect prompt type
        private bool IsSprintPrompt(string prompt)
        {
            return prompt.Contains("development sprints", StringComparison.OrdinalIgnoreCase)
                || prompt.Contains("sprint", StringComparison.OrdinalIgnoreCase)
                || prompt.Contains("module", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsTaskPrompt(string prompt)
        {
            return prompt.Contains("development tasks", StringComparison.OrdinalIgnoreCase)
                || prompt.Contains("step-by-step", StringComparison.OrdinalIgnoreCase)
                || prompt.Contains("task", StringComparison.OrdinalIgnoreCase);
        }


        private string GetEvaluationResponse()
        {
            return """
{
  "score": 82,
  "feedback": "Good implementation. Code structure is clean, but error handling can be improved."
}
""";
        }

        // 🧠 FAKE SPRINT RESPONSE
        private string GetSprintResponse()
        {
            return """
[
  {
    "sprintNumber": 1,
    "moduleName": "Project Setup & Database Design",
    "description": "Initialize backend project, setup database and basic structure.",
    "estimatedDays": 2
  },
  {
    "sprintNumber": 2,
    "moduleName": "Authentication Module",
    "description": "Implement login, registration and JWT authentication.",
    "estimatedDays": 3
  },
  {
    "sprintNumber": 3,
    "moduleName": "Core CRUD Operations",
    "description": "Build APIs and UI for main entity management.",
    "estimatedDays": 4
  },
  {
    "sprintNumber": 4,
    "moduleName": "UI Integration & Testing",
    "description": "Connect frontend with backend and perform testing.",
    "estimatedDays": 3
  }
]
""";
        }

        // 🧠 FAKE TASK RESPONSE
        private string GetTaskResponse()
        {
            return """
[
  {
    "taskNumber": 1,
    "task": "Create ASP.NET Core Web API project",
    "points": 20
  },
  {
    "taskNumber": 2,
    "task": "Setup database connection and create initial tables",
    "points": 25
  },
  {
    "taskNumber": 3,
    "task": "Implement repository and service layer",
    "points": 30
  },
  {
    "taskNumber": 4,
    "task": "Create API endpoints for CRUD operations",
    "points": 25
  },
  {
    "taskNumber": 5,
    "task": "Test APIs using Postman",
    "points": 15
  }
]
""";
        }
    }
}