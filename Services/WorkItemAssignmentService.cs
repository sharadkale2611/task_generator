using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
    public class WorkItemAssignmentService : IWorkItemAssignmentService
    {
        private readonly ApplicationDbContext _db;

        public WorkItemAssignmentService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 🔹 Assign / Reassign WorkItem
        public async Task<bool> AssignAsync(AssignWorkItemDto dto)
        {
            if (dto.WorkItemId <= 0)
                throw new Exception("Invalid WorkItem");

            if (dto.UserId <= 0)
                throw new Exception("Invalid User");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // 🔹 Validate WorkItem
                var workItem = await _db.WorkItems
                    .FirstOrDefaultAsync(w => w.WorkItemId == dto.WorkItemId);

                if (workItem == null)
                    throw new Exception("WorkItem not found");

                // 🔹 Validate User
                var user = await _db.Users
                    .FirstOrDefaultAsync(u => u.UserId == dto.UserId && !u.IsDeleted);

                if (user == null)
                    throw new Exception("User not found");

                // 🔹 Deactivate old assignment
                var existing = await _db.WorkItemAssignments
                    .FirstOrDefaultAsync(a => a.WorkItemId == dto.WorkItemId && a.IsActive);

                if (existing != null)
                {
                    existing.IsActive = false;
                    existing.UnassignedAt = DateTime.UtcNow;
                }

                // 🔹 Create new assignment
                var assignment = new WorkItemAssignment
                {
                    WorkItemId = dto.WorkItemId,
                    TenantId = user.TenantId,
                    AssignedToUserId = dto.UserId,
                    AssignedByUserId = dto.AssignedByUserId,
                    Reason = dto.Reason,
                    AssignedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _db.WorkItemAssignments.AddAsync(assignment);

                // 🔹 Update WorkItem (fast lookup)
                workItem.AssignedToUserId = dto.UserId;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // 🔹 Get Assignment History
        public async Task<List<WorkItemAssignmentResponseDto>> GetHistoryAsync(int workItemId)
        {
            return await _db.WorkItemAssignments
                .Where(a => a.WorkItemId == workItemId)
                .OrderByDescending(a => a.AssignedAt)
                .Select(a => new WorkItemAssignmentResponseDto
                {
                    WorkItemAssignmentId = a.WorkItemAssignmentId,
                    WorkItemId = a.WorkItemId,
                    WorkItemTitle = a.WorkItem.Title,
                    AssignedToUserId = a.AssignedToUserId,
                    AssignedToUserName = a.AssignedToUser.Name,
                    AssignedByUserId = a.AssignedByUserId,
                    Reason = a.Reason,
                    AssignedAt = a.AssignedAt,
                    UnassignedAt = a.UnassignedAt,
                    IsActive = a.IsActive
                })
                .ToListAsync();
        }

        // 🔹 Get Active Assignment
        public async Task<WorkItemAssignmentResponseDto?> GetActiveAssignmentAsync(int workItemId)
        {
            return await _db.WorkItemAssignments
                .Where(a => a.WorkItemId == workItemId && a.IsActive)
                .Select(a => new WorkItemAssignmentResponseDto
                {
                    WorkItemAssignmentId = a.WorkItemAssignmentId,
                    WorkItemId = a.WorkItemId,
                    WorkItemTitle = a.WorkItem.Title,
                    AssignedToUserId = a.AssignedToUserId,
                    AssignedToUserName = a.AssignedToUser.Name,
                    AssignedByUserId = a.AssignedByUserId,
                    Reason = a.Reason,
                    AssignedAt = a.AssignedAt,
                    UnassignedAt = a.UnassignedAt,
                    IsActive = a.IsActive
                })
                .FirstOrDefaultAsync();
        }
    }
}