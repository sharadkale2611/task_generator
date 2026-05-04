using AutoMapper;
using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Middlewares;
using task_generator.Models;

namespace task_generator.Services
{
	public class ProjectDomainService : IProjectDomainService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;

		public ProjectDomainService(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<ProjectDomainResponseDto> CreateAsync(ProjectDomainCreateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			if (string.IsNullOrWhiteSpace(dto.Name))
				throw new ArgumentException("Name is required");

			var name = dto.Name.Trim();

			bool exists = await _db.ProjectDomains
				.AnyAsync(x => x.Name.ToLower() == name.ToLower());

			if (exists)
				throw new InvalidOperationException("Project domain already exists");

			var entity = new ProjectDomain
			{
				Name = name,
				IsActive = true
			};

			await _db.ProjectDomains.AddAsync(entity);
			await _db.SaveChangesAsync();

			return _mapper.Map<ProjectDomainResponseDto>(entity);
		}

		public async Task<IEnumerable<ProjectDomainResponseDto>> GetAllAsync(bool? isActive)
		{
			var query = _db.ProjectDomains.AsQueryable();

			if (isActive.HasValue)
			{
				query = query.Where(x => x.IsActive == isActive.Value);
			}

			var data = await query.AsNoTracking().ToListAsync();

			return _mapper.Map<IEnumerable<ProjectDomainResponseDto>>(data);
		}

		public async Task<ProjectDomainResponseDto> UpdateAsync(int id, ProjectDomainUpdateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

			var entity = await _db.ProjectDomains.FindAsync(id);

			if (entity == null)
				throw new NotFoundException($"Project domain with id {id} not found");

			entity.Name = dto.Name.Trim();
			entity.IsActive = dto.IsActive;

			await _db.SaveChangesAsync();

			return _mapper.Map<ProjectDomainResponseDto>(entity);
		}
		public async Task<ProjectDomainResponseDto?> GetByIdAsync(int id)
		{
			var entity = await _db.ProjectDomains
				.FirstOrDefaultAsync(x => x.ProjectDomainId == id && x.IsActive);

			if (entity == null) return null;

			return _mapper.Map<ProjectDomainResponseDto>(entity);
		}

	}
}