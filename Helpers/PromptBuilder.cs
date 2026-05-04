using task_generator.Dto;
using System.Text;

namespace task_generator.Helpers
{
	public static class PromptBuilder
	{
		private static readonly List<string> Domains = new()
		{
			"E-commerce","Hospital","Library","Inventory",
			"School","Booking","HR","Billing"
		};

		public static string Build(ProjectGeneratorRequest request)
		{
			var sb = new StringBuilder();

			var index = Math.Abs(Guid.NewGuid().GetHashCode()) % Domains.Count;
			var domain = string.IsNullOrWhiteSpace(request.Domain)
				? Domains[index]
				: request.Domain;

			var isCalculation = request.ProjectType?.ToLower().Contains("calculation") == true;

			sb.AppendLine("You are a senior software trainer.");

			// 🔥 MODE SWITCH
			if (request.IsPreview)
			{
				sb.AppendLine("Generate ONLY a unique beginner-friendly project idea.");
				sb.AppendLine("Do NOT generate tasks.");
			}
			else
			{
				sb.AppendLine("Generate a beginner-friendly mini project task breakdown.");
			}

			sb.AppendLine();

			sb.AppendLine("Project Requirements:");
			sb.AppendLine($"Tech Stack: {request.TechStack}");
			sb.AppendLine($"Student Level: {request.Level}");
			sb.AppendLine($"Duration: {request.DurationDays} days");
			sb.AppendLine($"Project Type: {request.ProjectType}");
			sb.AppendLine($"Project Domain: {domain}");
			sb.AppendLine();

			// IDEA RULES
			sb.AppendLine("Project Idea Rules:");
			sb.AppendLine("- Generate a UNIQUE projectName every time");
			sb.AppendLine("- Do NOT repeat common projects like Student Management System");
			sb.AppendLine($"- Project MUST belong to domain: {domain}");
			sb.AppendLine("- Keep project simple and beginner friendly");

			if (isCalculation)
			{
				sb.AppendLine("- Project MUST be based on custom calculations or formulas");
				sb.AppendLine("- Include logic like mathematical formulas or computed outputs");
				sb.AppendLine("- Avoid pure CRUD-only projects");
			}
			else
			{
				sb.AppendLine("- Project must be CRUD based");
			}

			sb.AppendLine();

			// Avoid duplicates
			if (request.PreviousProjects?.Any() == true)
			{
				sb.AppendLine("Avoid generating these project names:");
				foreach (var p in request.PreviousProjects)
					sb.AppendLine($"- {p}");

				sb.AppendLine();
			}

			// 🔥 PREVIEW MODE OUTPUT
			if (request.IsPreview)
			{
				sb.AppendLine("CRITICAL OUTPUT RULES:");
				sb.AppendLine("- Return ONLY valid JSON");
				sb.AppendLine("- No explanation text");
				sb.AppendLine("- Output must be a JSON object");
				sb.AppendLine("- Use camelCase");
				sb.AppendLine();

				sb.AppendLine("JSON Structure:");
				sb.AppendLine("{");
				sb.AppendLine(@"  ""projectName"": ""string"",");
				sb.AppendLine(@"  ""description"": ""string"",");
				sb.AppendLine(@"  ""projectType"": ""string"",");
				sb.AppendLine(@"  ""domain"": ""string""");
				sb.AppendLine("}");

				return sb.ToString();
			}

			// 🔥 FULL TASK MODE
			sb.AppendLine("Rules:");
			sb.AppendLine("- Avoid advanced concepts");
			sb.AppendLine("- Tasks must be simple");
			sb.AppendLine("- Distribute tasks across days");
			sb.AppendLine("- Assign points (10-30)");
			sb.AppendLine("- Week format: Week 1: BACKEND API");
			sb.AppendLine("- Day format: Day 1 - Module Name");

			if (isCalculation)
			{
				sb.AppendLine("- Include calculation logic implementation steps");
			}

			sb.AppendLine();

			sb.AppendLine("CRITICAL OUTPUT RULES:");
			sb.AppendLine("- Return ONLY valid JSON array");
			sb.AppendLine("- No explanation text");
			sb.AppendLine("- No markdown");
			sb.AppendLine("- No comments");
			sb.AppendLine("- Use camelCase");

			sb.AppendLine();

			sb.AppendLine("JSON Structure:");
			sb.AppendLine("{");
			sb.AppendLine(@"  ""projectName"": ""string"",");
			sb.AppendLine(@"  ""techStack"": ""string"",");
			sb.AppendLine(@"  ""level"": ""string"",");
			sb.AppendLine(@"  ""week"": ""string"",");
			sb.AppendLine(@"  ""day"": ""string"",");
			sb.AppendLine(@"  ""taskNumber"": number,");
			sb.AppendLine(@"  ""task"": ""string"",");
			sb.AppendLine(@"  ""points"": number");
			sb.AppendLine("}");

			sb.AppendLine();
			sb.AppendLine("Start with [ and end with ]");
			sb.AppendLine("Generate full task breakdown now.");

			return sb.ToString();
		}
	}
}