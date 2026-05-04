using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using task_generator.AI;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAiClient _aiClient;
        private readonly IGitHubService _gitHubService;

        public EvaluationService(
            ApplicationDbContext db,
            IAiClient aiClient,
            IGitHubService gitHubService)
        {
            _db = db;
            _aiClient = aiClient;
            _gitHubService = gitHubService;
        }

        public async Task<EvaluationResponseDto> EvaluateAsync(int submissionId)
        {
            var submission = await _db.Submissions
                .Include(s => s.WorkItem)
                .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

            if (submission == null)
                throw new Exception("Submission not found");

            var existing = await _db.Evaluations
                .FirstOrDefaultAsync(e => e.SubmissionId == submissionId);

            if (existing != null)
            {
                return new EvaluationResponseDto
                {
                    EvaluationId = existing.EvaluationId,
                    SubmissionId = existing.SubmissionId,
                    Score = existing.Score,
                    Feedback = existing.Feedback,
                    CreatedAt = existing.CreatedAt
                };
            }

            // 🔥 GitHub Integration
            var files = await _gitHubService.GetChangedFiles(
                submission.RepoUrl!,
                submission.BaseBranch!,
                submission.BranchName!
            );

            var fileContents = await _gitHubService.GetFileContents(
                submission.RepoUrl!,
                submission.BranchName!,
                files
            );

            var changedFilesSummary = string.Join("\n",
                files.Select(f => $"- {f.FilePath} ({f.Status})"));

            var codeSnippets = string.Join("\n\n",
                fileContents.Select(f => $@"
File: {f.FilePath}
-------------------
{f.Content}
"));

            // 🔥 Prompt
            var prompt = $@"
You are an expert software reviewer.

TASK:
{submission.WorkItem.Title}

DESCRIPTION:
{submission.WorkItem.Description}

INTERN EXPLANATION:
{submission.SubmissionNotes ?? "No explanation provided"}

CHANGED FILES:
{changedFilesSummary}

CODE:
{codeSnippets}

Return JSON:
{{
  ""score"": number,
  ""feedback"": ""short feedback""
}}
";

            Console.WriteLine("====== AI PROMPT START ======");
            Console.WriteLine(prompt);
            Console.WriteLine("====== AI PROMPT END ======");

            var aiResponse = await _aiClient.GetCompletionAsync(prompt);

            var result = JsonSerializer.Deserialize<EvaluationResult>(aiResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null)
                throw new Exception("AI parsing failed");

            var evaluation = new Evaluation
            {
                SubmissionId = submissionId,
                TenantId = submission.TenantId,
                Score = result.Score,
                Feedback = result.Feedback
            };

            await _db.Evaluations.AddAsync(evaluation);

            submission.Status = "Reviewed";

            await _db.SaveChangesAsync();

            return new EvaluationResponseDto
            {
                EvaluationId = evaluation.EvaluationId,
                SubmissionId = submissionId,
                Score = evaluation.Score,
                Feedback = evaluation.Feedback,
                CreatedAt = evaluation.CreatedAt
            };
        }

        public async Task<EvaluationResponseDto?> GetBySubmissionAsync(int submissionId)
        {
            return await _db.Evaluations
                .Where(e => e.SubmissionId == submissionId)
                .Select(e => new EvaluationResponseDto
                {
                    EvaluationId = e.EvaluationId,
                    SubmissionId = e.SubmissionId,
                    Score = e.Score,
                    Feedback = e.Feedback,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
    }

    public class EvaluationResult
    {
        public int Score { get; set; }
        public string Feedback { get; set; } = "";
    }
}