using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
	public class EpicService : IEpicService
	{
		private readonly ApplicationDbContext _db;

		public EpicService(ApplicationDbContext db)
		{
			_db = db;			
		}
		public async Task<List<string>> GetProjectNamesAsync(int categoryId, int domainId)
		{
			return await _db.Epics
				.Where(e => e.ProjectCategoryId == categoryId
						 && e.ProjectDomainId == domainId)
				.Select(e => e.ProjectName)
				.ToListAsync();
		}


		public async Task<EpicResponseDto> CreateAsync(CreateEpicDto dto)
		{
			var epic = new Epic
			{
				ProjectName = dto.ProjectName,
				Description = dto.Description,
				ProjectCategoryId = dto.ProjectCategoryId,
				ProjectDomainId = dto.ProjectDomainId,
				Status = "Generated",
				Level = dto.Level.ToString(),
				EstimatedDuration = dto.EstimatedDuration,
				Source = dto.Source,
				IsApproved = false
			};

			await _db.Epics.AddAsync(epic);
			await _db.SaveChangesAsync();

			// 🔥 TechStack mapping
			if (dto.TechStackIds.Any())
			{
				var mappings = dto.TechStackIds.Select(ts => new EpicTechStack
				{
					EpicId = epic.EpicId,
					TechStackId = ts
				});

				await _db.EpicTechStacks.AddRangeAsync(mappings);
				await _db.SaveChangesAsync();
			}

			return await MapToResponse(epic.EpicId);
		}

		public async Task<EpicResponseDto> UpdateAsync(UpdateEpicDto dto)
		{
			var epic = await _db.Epics
				.Include(e => e.EpicTechStacks)
				.FirstOrDefaultAsync(e => e.EpicId == dto.EpicId);

			if (epic == null)
				throw new Exception("Epic not found");

			epic.ProjectName = dto.ProjectName;
			epic.Description = dto.Description;
			epic.ProjectCategoryId = dto.ProjectCategoryId;
			epic.ProjectDomainId = dto.ProjectDomainId;
			epic.Level = dto.Level.ToString();
			epic.EstimatedDuration = dto.EstimatedDuration;
			epic.IsApproved = dto.IsApproved;

			// 🔥 Update TechStacks
			_db.EpicTechStacks.RemoveRange(epic.EpicTechStacks);

			var newMappings = dto.TechStackIds.Select(ts => new EpicTechStack
			{
				EpicId = epic.EpicId,
				TechStackId = ts
			});

			await _db.EpicTechStacks.AddRangeAsync(newMappings);

			await _db.SaveChangesAsync();

			return await MapToResponse(epic.EpicId);
		}

		public async Task<bool> SoftDeleteAsync(int epicId)
		{
			var epic = await _db.Epics.FindAsync(epicId);

			if (epic == null)
				return false;

			epic.Status = "Deleted";

			await _db.SaveChangesAsync();
			return true;
		}

		private async Task<EpicResponseDto> MapToResponse(int epicId)
		{
			var epic = await _db.Epics
				.Include(e => e.ProjectCategory)
				.Include(e => e.ProjectDomain)
				.Include(e => e.EpicTechStacks)
					.ThenInclude(et => et.TechStack)
				.FirstAsync(e => e.EpicId == epicId);

			return new EpicResponseDto
			{
				EpicId = epic.EpicId,
				ProjectName = epic.ProjectName,
				Description = epic.Description,
				ProjectCategory = epic.ProjectCategory.Name,
				Domain = epic.ProjectDomain.Name,
				Status = epic.Status,
				Level = epic.Level,
				EstimatedDuration = epic.EstimatedDuration,
				Source = epic.Source,
				IsApproved = epic.IsApproved,
				TechStacks = epic.EpicTechStacks.Select(x => x.TechStack.Name).ToList()
			};
		}

		public async Task<bool> ExistsAsync(string projectName, int categoryId, int domainId)
		{
			return await _db.Epics.AnyAsync(e =>
				e.ProjectName.ToLower() == projectName.ToLower() &&
				e.ProjectCategoryId == categoryId &&
				e.ProjectDomainId == domainId &&
				e.Status != "Deleted"
			);
		}

		public async Task<List<EpicResponseDto>> GetAllAsync(string? search, string? status)
		{
			var query = _db.Epics
				.Include(e => e.ProjectCategory)
				.Include(e => e.ProjectDomain)
				.Include(e => e.EpicTechStacks)
					.ThenInclude(et => et.TechStack)
				.Where(e => e.Status != "Deleted")
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(search))
			{
				search = search.ToLower();
				query = query.Where(e =>
					e.ProjectName.ToLower().Contains(search) ||
					e.Description.ToLower().Contains(search));
			}

			if (!string.IsNullOrWhiteSpace(status) && status != "All")
			{
				query = query.Where(e => e.Status == status);
			}

			var data = await query.ToListAsync();

			return data.Select(e => new EpicResponseDto
			{
				EpicId = e.EpicId,
				ProjectName = e.ProjectName,
				Description = e.Description,
				ProjectCategory = e.ProjectCategory.Name,
				Domain = e.ProjectDomain.Name,
				Status = e.Status,
				Level = e.Level,
				EstimatedDuration = e.EstimatedDuration,
				Source = e.Source,
				IsApproved = e.IsApproved,
				TechStacks = e.EpicTechStacks.Select(x => x.TechStack.Name).ToList()
			}).ToList();
		}



	}
}
