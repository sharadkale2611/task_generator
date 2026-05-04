using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Middlewares;
using task_generator.Models;

namespace task_generator.Services
{
	public class TechStackService : ITechStackService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;


		public TechStackService(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}
		public async Task<TechStackResponseDto> CreateAsync(TechStackCreateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentNullException(nameof(dto.Name));
			if (string.IsNullOrWhiteSpace(dto.Category)) throw new ArgumentNullException(nameof(dto.Category));

			bool exists = await _db.TechStacks
				.AnyAsync(tt => tt.Name == dto.Name && tt.Category == dto.Category);

			if (exists)
				throw new InvalidOperationException("Tech stack already exists");

			var entity = new TechStack
			{
				Name = dto.Name,
				Category = dto.Category,
				IsActive = true
			};

			await _db.TechStacks.AddAsync(entity);
			await _db.SaveChangesAsync();

			return _mapper.Map<TechStackResponseDto>(entity);
		}

		public async Task<IEnumerable<TechStackResponseDto>> GetAllAsync(bool? isActive)
		{
			var query = _db.TechStacks.AsQueryable();

			if (isActive.HasValue)
			{
				query = query.Where(tt => tt.IsActive == isActive.Value);
			}

			var entity =  await query.ToListAsync();
			return _mapper.Map<IEnumerable<TechStackResponseDto>>(entity);
		}

		public async Task<TechStackResponseDto> UpdateAsync(int id, TechStackUpdateDto dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));
			if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

			TechStack? techStack = await _db.TechStacks.FindAsync(id);

			if (techStack == null) throw new NotFoundException($"Tech stack with id {id} not found");

			techStack.Name = dto.Name;
			techStack.Category = dto.Category;
			techStack.IsActive = dto.IsActive;

			await _db.SaveChangesAsync();

			return _mapper.Map<TechStackResponseDto>(techStack);

		}

		public async Task<List<TechStackResponseDto>> GetByIdsAsync(List<int> ids)
		{
			if (ids == null || !ids.Any())
				return new List<TechStackResponseDto>();

			var entities = await _db.TechStacks
				.Where(x => ids.Contains(x.TechStackId) && x.IsActive)
				.ToListAsync();

			return _mapper.Map<List<TechStackResponseDto>>(entities);
		}
	}
}
