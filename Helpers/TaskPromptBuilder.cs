using System.Text;

namespace task_generator.Helpers
{
	public static class TaskPromptBuilder
	{
		public static string Build(
			string projectName,
			string moduleName,
			string moduleDescription,
			string level)
		{
			var sb = new StringBuilder();

			sb.AppendLine("You are a senior software trainer.");
			sb.AppendLine("Generate step-by-step development tasks for the given module.");
			sb.AppendLine();

			sb.AppendLine($"Project: {projectName}");
			sb.AppendLine($"Module: {moduleName}");
			sb.AppendLine($"Module Description: {moduleDescription}");
			sb.AppendLine($"Student Level: {level}");
			sb.AppendLine();

			sb.AppendLine("Rules:");
			sb.AppendLine("- Tasks must be simple and actionable");
			sb.AppendLine("- Follow logical development order");
			sb.AppendLine("- Each task should take 1-3 hours");
			sb.AppendLine("- Include coding + testing steps");
			sb.AppendLine("- Assign points (10-30)");
			sb.AppendLine();

			sb.AppendLine("CRITICAL OUTPUT RULES:");
			sb.AppendLine("- Return ONLY valid JSON array");
			sb.AppendLine("- No explanation");
			sb.AppendLine("- Use camelCase");
			sb.AppendLine();

			sb.AppendLine("JSON Structure:");
			sb.AppendLine("[");
			sb.AppendLine(@"  {");
			sb.AppendLine(@"    ""title"": ""string"",");
			sb.AppendLine(@"    ""points"": number,");
			sb.AppendLine(@"    ""subTasks"": [");
			sb.AppendLine(@"      {");
			sb.AppendLine(@"        ""title"": ""string"",");
			sb.AppendLine(@"        ""points"": number");
			sb.AppendLine(@"      }");
			sb.AppendLine(@"    ]");
			sb.AppendLine(@"  }");
			sb.AppendLine("]");

			return sb.ToString();
		}
	}
}
