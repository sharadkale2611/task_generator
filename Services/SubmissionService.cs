using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ApplicationDbContext _db;

        public SubmissionService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 🔹 Create Submission
        public async Task<SubmissionResponseDto> CreateAsync(CreateSubmissionDto dto)
        {
            // 🔥 Validation: at least one required
            if (string.IsNullOrWhiteSpace(dto.RepoUrl) &&
                string.IsNullOrWhiteSpace(dto.SubmissionNotes))
            {
                throw new Exception("Either RepoUrl or SubmissionNotes is required");
            }

            var workItem = await _db.WorkItems
                .FirstOrDefaultAsync(w => w.WorkItemId == dto.WorkItemId);

            if (workItem == null)
                throw new Exception("WorkItem not found");

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId && !u.IsDeleted);

            if (user == null)
                throw new Exception("User not found");

            // 🔥 Validate assignment
            var isAssigned = await _db.WorkItemAssignments
                .AnyAsync(a => a.WorkItemId == dto.WorkItemId &&
                               a.AssignedToUserId == dto.UserId &&
                               a.IsActive);

            if (!isAssigned)
                throw new Exception("User is not assigned to this task");

            // 🔥 Validate repo fields if repo is provided
            if (!string.IsNullOrWhiteSpace(dto.RepoUrl))
            {
                if (string.IsNullOrWhiteSpace(dto.BranchName))
                    throw new Exception("BranchName is required when RepoUrl is provided");

                if (string.IsNullOrWhiteSpace(dto.BaseBranch))
                    dto.BaseBranch = "main"; // default
            }

            var submission = new Submission
            {
                WorkItemId = dto.WorkItemId,
                UserId = dto.UserId,
                TenantId = user.TenantId,

                SubmissionNotes = dto.SubmissionNotes,

                RepoUrl = dto.RepoUrl,
                BranchName = dto.BranchName,
                BaseBranch = dto.BaseBranch,

                Status = "Submitted",
                CreatedAt = DateTime.UtcNow
            };

            await _db.Submissions.AddAsync(submission);
            await _db.SaveChangesAsync();

            return new SubmissionResponseDto
            {
                SubmissionId = submission.SubmissionId,
                WorkItemId = workItem.WorkItemId,
                WorkItemTitle = workItem.Title,
                UserId = user.UserId,
                UserName = user.Name,

                SubmissionNotes = submission.SubmissionNotes,
                RepoUrl = submission.RepoUrl,
                BranchName = submission.BranchName,
                BaseBranch = submission.BaseBranch,

                Status = submission.Status,
                CreatedAt = submission.CreatedAt
            };
        }

        // 🔹 Get by User
        public async Task<List<SubmissionResponseDto>> GetByUserAsync(int userId)
        {
            return await _db.Submissions
                .Where(s => s.UserId == userId)
                .Select(s => new SubmissionResponseDto
                {
                    SubmissionId = s.SubmissionId,
                    WorkItemId = s.WorkItemId,
                    WorkItemTitle = s.WorkItem.Title,
                    UserId = s.UserId,
                    UserName = s.User.Name,

                    SubmissionNotes = s.SubmissionNotes,
                    RepoUrl = s.RepoUrl,
                    BranchName = s.BranchName,
                    BaseBranch = s.BaseBranch,

                    Status = s.Status,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();
        }

        // 🔹 Get by WorkItem
        public async Task<List<SubmissionResponseDto>> GetByWorkItemAsync(int workItemId)
        {
            return await _db.Submissions
                .Where(s => s.WorkItemId == workItemId)
                .Select(s => new SubmissionResponseDto
                {
                    SubmissionId = s.SubmissionId,
                    WorkItemId = s.WorkItemId,
                    WorkItemTitle = s.WorkItem.Title,
                    UserId = s.UserId,
                    UserName = s.User.Name,

                    SubmissionNotes = s.SubmissionNotes,
                    RepoUrl = s.RepoUrl,
                    BranchName = s.BranchName,
                    BaseBranch = s.BaseBranch,

                    Status = s.Status,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();
        }
    }
}