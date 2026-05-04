using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using task_generator.AI;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Helpers;
using task_generator.Models;

namespace task_generator.Services
{
    public class SprintService : ISprintService
    {
        private readonly ApplicationDbContext _db;
        private readonly ISprintCreateResponse _sprintCreator;

        public SprintService(ApplicationDbContext db, ISprintCreateResponse sprintCreator)
        {
            _db = db;
            _sprintCreator = sprintCreator;
        }

        // 🔥 GENERATE SPRINTS (AI → DB)
        public async Task<List<Sprint>> GenerateAsync(int epicId)
        {
            var project = await _db.Epics.FindAsync(epicId);

            if (project == null)
                throw new Exception("Project not found");

            // ❗ Prevent duplicate generation
            bool alreadyExists = await _db.Sprints
                .AnyAsync(s => s.EpicId == epicId);

            if (alreadyExists)
                throw new Exception("Sprints already generated for this project");

            // 🔥 Convert duration (string → days)
            int durationDays = ParseDuration(project.EstimatedDuration);

            // 🔥 Build prompt
            var prompt = SprintPromptBuilder.Build(
                project.ProjectName,
                project.Description,
                durationDays,
                project.Level
            );

            // 🔥 Call AI
            var aiResponse = await _sprintCreator.Generate(prompt);

            // 🔥 Safe Deserialize
            List<SprintDto>? sprintDtos;
            try
            {
                sprintDtos = JsonSerializer.Deserialize<List<SprintDto>>(
                    aiResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );
            }
            catch
            {
                throw new Exception("AI response parsing failed");
            }

            if (sprintDtos == null || !sprintDtos.Any())
                throw new Exception("AI returned invalid sprint data");

            // 🔥 VALIDATION 1: Individual sprint validation
            foreach (var dto in sprintDtos)
            {
                if (dto.EstimatedDays <= 0)
                    throw new Exception($"Invalid days in Sprint {dto.SprintNumber}");

                if (string.IsNullOrWhiteSpace(dto.ModuleName))
                    throw new Exception($"Sprint {dto.SprintNumber} has empty name");
            }

            // 🔥 VALIDATION 2: Total duration validation
            int totalAiDays = sprintDtos.Sum(x => x.EstimatedDays);

            if (totalAiDays > durationDays + 2) // tolerance buffer
                throw new Exception("AI generated inconsistent sprint duration");

            // 🔥 Map to Entity
            var sprints = new List<Sprint>();
            DateTime startDate = DateTime.UtcNow;

            foreach (var dto in sprintDtos.OrderBy(x => x.SprintNumber))
            {
                var endDate = startDate.AddDays(dto.EstimatedDays);

                sprints.Add(new Sprint
                {
                    EpicId = epicId,
                    Name = dto.ModuleName,
                    StartDate = startDate,
                    EndDate = endDate,
                    Order = dto.SprintNumber,
                    Status = "Planned"
                });

                startDate = endDate;
            }

            // 🔥 TRANSACTION SAFETY
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                await _db.Sprints.AddRangeAsync(sprints);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return sprints;
        }

        // 🔥 GET SPRINTS BY PROJECT
        public async Task<List<Sprint>> GetByEpicIdAsync(int epicId)
        {
            var epicExists = await _db.Epics.AnyAsync(e => e.EpicId == epicId);

            if (!epicExists)
                throw new Exception($"Epic with ID {epicId} does not exist");

            return await _db.Sprints
                .Where(s => s.EpicId == epicId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        // 🔧 HELPER: Convert "2 weeks" → 14 days
        private int ParseDuration(string duration)
        {
            if (string.IsNullOrWhiteSpace(duration))
                return 14;

            duration = duration.ToLower();

            if (duration.Contains("week"))
            {
                var number = ExtractNumber(duration);
                return number * 7;
            }

            if (duration.Contains("day"))
            {
                return ExtractNumber(duration);
            }

            return 14; // fallback
        }

        private int ExtractNumber(string input)
        {
            var digits = new string(input.Where(char.IsDigit).ToArray());

            if (int.TryParse(digits, out int value))
                return value;

            return 2; // default fallback
        }
    }
}