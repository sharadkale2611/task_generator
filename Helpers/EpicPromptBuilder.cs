using System.Text;
using task_generator.Dto;

namespace task_generator.Helpers
{
	public static class EpicPromptBuilder
	{
		public static string Build(EpicAIContext context)
		{
			var sb = new StringBuilder();

			sb.AppendLine("You are a senior software architect and trainer.");
			sb.AppendLine("Generate 3 unique real-world software project ideas.");
			sb.AppendLine();

			// 🔹 REQUIREMENTS
			sb.AppendLine("Requirements:");
			sb.AppendLine($"Tech Stack: {context.TechStack}");
			sb.AppendLine($"Student Level: {context.Level}");
			sb.AppendLine($"Project Category: {context.ProjectCategory}");
			sb.AppendLine($"Domain: {context.Domain}");

			if (!string.IsNullOrWhiteSpace(context.Goal))
				sb.AppendLine($"Goal: {context.Goal}");

			if (!string.IsNullOrWhiteSpace(context.IndustryFocus))
				sb.AppendLine($"Industry Focus: {context.IndustryFocus}");

			sb.AppendLine();

			// 🔹 RULES
			sb.AppendLine("Rules:");
			sb.AppendLine("- Each project must be unique");
			sb.AppendLine("- Avoid common systems like Student Management System, Todo App");
			sb.AppendLine("- Keep projects practical and industry-relevant");
			sb.AppendLine("- Match difficulty with student level");
			sb.AppendLine("- Prefer real-world use cases");

			if (context.ProjectCategory.ToLower().Contains("calculation"))
			{
				sb.AppendLine("- Include meaningful calculations or business logic");
				sb.AppendLine("- Avoid simple CRUD-only projects");
			}
			else if (context.ProjectCategory.ToLower().Contains("crud"))
			{
				sb.AppendLine("- Project should include proper CRUD operations");
			}

			sb.AppendLine();

			// 🔹 AVOID DUPLICATES
			if (context.PreviousProjects?.Any() == true)
			{
				sb.AppendLine("Avoid generating these project names:");
				foreach (var p in context.PreviousProjects)
					sb.AppendLine($"- {p}");

				sb.AppendLine();
			}

			// 🔹 OUTPUT RULES
			sb.AppendLine("CRITICAL OUTPUT RULES:");
			sb.AppendLine("- Return ONLY valid JSON array");
			sb.AppendLine("- No explanation text");
			sb.AppendLine("- No markdown");
			sb.AppendLine("- No comments");
			sb.AppendLine("- Use camelCase");
			sb.AppendLine("- Do NOT include trailing commas");
			sb.AppendLine("- Do NOT include null values");
			sb.AppendLine();

			sb.AppendLine("VERY IMPORTANT:");
			sb.AppendLine("- techStack MUST NOT be empty");
			sb.AppendLine("- techStack must contain 2-4 technologies");
			sb.AppendLine("- Use ONLY technologies from this list:");
			sb.AppendLine(context.TechStack);
			sb.AppendLine("- If unsure, reuse technologies from the list above");
			sb.AppendLine("- Do NOT invent new technologies");
			sb.AppendLine();

			sb.AppendLine("STRICT JSON FORMAT:");
			sb.AppendLine("- Always include ALL fields");
			sb.AppendLine("- Field names must match EXACTLY as below");
			sb.AppendLine("- techStack must always be an array of strings");
			sb.AppendLine();

			sb.AppendLine("JSON Structure:");
			sb.AppendLine("[");
			sb.AppendLine(@"  {");
			sb.AppendLine(@"    ""projectName"": ""string"",");
			sb.AppendLine(@"    ""description"": ""string"",");
			sb.AppendLine(@"    ""projectCategory"": ""string"",");
			sb.AppendLine(@"    ""domain"": ""string"",");
			sb.AppendLine(@"    ""level"": ""Beginner"",");
			sb.AppendLine(@"    ""estimatedDuration"": ""string"",");
			sb.AppendLine(@"    ""techStack"": [""React"", "".Net Core API""],");
			sb.AppendLine(@"    ""status"": ""Generated"",");
			sb.AppendLine(@"    ""source"": ""AI""");
			sb.AppendLine(@"  }");
			sb.AppendLine("]");
			return sb.ToString();
		}
	}
}