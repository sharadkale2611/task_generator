using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using task_generator.AI;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Helpers;
using task_generator.Models;

namespace task_generator.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITaskCreateResponse _taskCreator;

        public WorkItemService(
            ApplicationDbContext db,
            ITaskCreateResponse taskCreator)
        {
            _db = db;
            _taskCreator = taskCreator;
        }

        // 🔥 GENERATE + SAVE HIERARCHICAL WORK ITEMS
        public async Task<List<WorkItem>> GenerateAsync(int sprintId)
        {
            var sprint = await _db.Sprints
                .FirstOrDefaultAsync(s => s.SprintId == sprintId);

            if (sprint == null)
                throw new Exception("Sprint not found");

            var project = await _db.Epics
                .FirstOrDefaultAsync(p => p.EpicId == sprint.EpicId);

            if (project == null)
                throw new Exception("Project not found");

            // ❗ Idempotent check
            bool alreadyExists = await _db.WorkItems
                .AnyAsync(w => w.SprintId == sprintId);

            if (alreadyExists)
            {
                return await GetBySprintIdAsync(sprintId);
            }

            // 🔥 Build Prompt
            var prompt = TaskPromptBuilder.Build(
                project.ProjectName,
                sprint.Name,
                "", // optional
                project.Level
            );

            // 🔥 Call AI
            var aiResponse = await _taskCreator.Generate(prompt);

            // 🔥 Safe Deserialize
            List<TaskHierarchyDto>? dtos;

            try
            {
                dtos = JsonSerializer.Deserialize<List<TaskHierarchyDto>>(
                    aiResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );
            }
            catch
            {
                throw new Exception("AI response parsing failed");
            }

            if (dtos == null || !dtos.Any())
                throw new Exception("AI returned invalid task data");

            var workItems = new List<WorkItem>();
            int order = 1;

            foreach (var dto in dtos)
            {
                var parent = CreateWorkItemRecursive(
                    dto,
                    project.EpicId,
                    sprintId,
                    null
                );

                parent.Order = order++; // 🔥 maintain order
                workItems.Add(parent);
            }

            // 🔥 TRANSACTION SAFETY
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                await _db.WorkItems.AddRangeAsync(workItems);
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return workItems;
        }

        // 🔥 GET WORK ITEMS BY SPRINT (Hierarchy)
        public async Task<List<WorkItem>> GetBySprintIdAsync(int sprintId)
        {
            return await _db.WorkItems
                .Where(w => w.SprintId == sprintId && w.ParentWorkItemId == null)
                .Include(w => w.Children)
                    .ThenInclude(c => c.Children)
                .OrderBy(w => w.Order)
                .ToListAsync();
        }

        // 🔥 RECURSIVE CREATION
        private WorkItem CreateWorkItemRecursive(
            TaskHierarchyDto dto,
            int projectId,
            int sprintId,
            WorkItem? parent)
        {
            var current = new WorkItem
            {
                ProjectId = projectId,
                SprintId = sprintId,
                Title = dto.Title,
                Description = dto.Title,
                Type = parent == null ? "Task" : "Subtask",
                Status = "ToDo",
                Points = dto.Points > 0 ? dto.Points : 1,
                Parent = parent // ✅ EF handles FK automatically
            };

            if (dto.SubTasks != null && dto.SubTasks.Any())
            {
                foreach (var sub in dto.SubTasks)
                {
                    var child = CreateWorkItemRecursive(
                        sub,
                        projectId,
                        sprintId,
                        current
                    );

                    current.Children.Add(child);
                }
            }

            return current;
        }


        public async Task<bool> UpdateStatusAsync(int workItemId, string status)
        {
            var item = await _db.WorkItems.FindAsync(workItemId);

            if (item == null)
                return false;

            // 🔥 Validate allowed statuses
            var allowedStatuses = new[] { "ToDo", "InProgress", "Done" };

            if (!allowedStatuses.Contains(status))
                throw new Exception("Invalid status value");

            item.Status = status;

            await _db.SaveChangesAsync();

            return true;
        }



    }
}