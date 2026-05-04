using AutoMapper;
using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Middlewares;
using task_generator.Models;

namespace task_generator.Services
{
	public class ProjectCategoryService : IProjectCategoryService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		public ProjectCategoryService(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<ProjectCategoryResponseDto> CreateAsync(ProjectCategoryCreateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required");

			bool exists = await _db.ProjectCategories
				.AnyAsync(x => x.Name == dto.Name);

			if (exists)
				throw new InvalidOperationException("Project category already exists");

			var entity = new ProjectCategory
			{
				Name = dto.Name,
				IsActive = true
			};

			await _db.ProjectCategories.AddAsync(entity);
			await _db.SaveChangesAsync();

			return _mapper.Map<ProjectCategoryResponseDto>(entity);
		}

		public async Task<IEnumerable<ProjectCategoryResponseDto>> GetAllAsync(bool? isActive)
		{
			var query = _db.ProjectCategories.AsQueryable();

			if (isActive.HasValue)
			{
				query = query.Where(x => x.IsActive == isActive.Value);
			}

			var data = await query.ToListAsync();

			return _mapper.Map<IEnumerable<ProjectCategoryResponseDto>>(data);
		}

		public async Task<ProjectCategoryResponseDto> UpdateAsync(int id, ProjectCategoryUpdateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

			var entity = await _db.ProjectCategories.FindAsync(id);

			if (entity == null)
				throw new NotFoundException($"Project category with id {id} not found");

			entity.Name = dto.Name;
			entity.IsActive = dto.IsActive;

			await _db.SaveChangesAsync();

			return _mapper.Map<ProjectCategoryResponseDto>(entity);
		}

		public async Task<ProjectCategoryResponseDto?> GetByIdAsync(int id)
		{
			var entity = await _db.ProjectCategories
				.FirstOrDefaultAsync(x => x.ProjectCategoryId == id && x.IsActive);

			if (entity == null) return null;

			return _mapper.Map<ProjectCategoryResponseDto>(entity);
		}
	}
}
