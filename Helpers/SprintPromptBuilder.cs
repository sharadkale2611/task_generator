using System.Text;

namespace task_generator.Helpers
{
	public static class SprintPromptBuilder
	{
		public static string Build(string projectName, string description, int durationDays, string level)
		{
			var sb = new StringBuilder();

			sb.AppendLine("You are a senior software architect and Agile coach.");
			sb.AppendLine("Break the project into well-structured development sprints.");
			sb.AppendLine();

			sb.AppendLine($"Project: {projectName}");
			sb.AppendLine($"Description: {description}");
			sb.AppendLine($"Total Duration: {durationDays} days");
			sb.AppendLine($"Student Level: {level}");
			sb.AppendLine();

			sb.AppendLine("Rules:");
			sb.AppendLine("- Divide project into logical sprints (modules)");
			sb.AppendLine("- Each sprint should represent a real feature or milestone");
			sb.AppendLine("- Each sprint duration must be between 2-5 days");
			sb.AppendLine("- Cover full project from setup → deployment");
			sb.AppendLine("- Follow correct development order");
			sb.AppendLine("- Include UI, backend, and integration where applicable");
			sb.AppendLine("- Keep difficulty suitable for the student level");
			sb.AppendLine();

			sb.AppendLine("VERY IMPORTANT:");
			sb.AppendLine("- Total estimatedDays should be close to total duration");
			sb.AppendLine("- Do NOT skip essential steps like authentication, APIs, or testing");
			sb.AppendLine();

			sb.AppendLine("CRITICAL OUTPUT RULES:");
			sb.AppendLine("- Return ONLY valid JSON array");
			sb.AppendLine("- No explanation text");
			sb.AppendLine("- No markdown");
			sb.AppendLine("- No trailing commas");
			sb.AppendLine("- Use camelCase");
			sb.AppendLine();

			sb.AppendLine("JSON Structure:");
			sb.AppendLine("[");
			sb.AppendLine(@"  {");
			sb.AppendLine(@"    ""sprintNumber"": number,");
			sb.AppendLine(@"    ""moduleName"": ""string"",");
			sb.AppendLine(@"    ""description"": ""string"",");
			sb.AppendLine(@"    ""estimatedDays"": number");
			sb.AppendLine(@"  }");
			sb.AppendLine("]");

			return sb.ToString();
		}
	}

}
	