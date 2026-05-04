using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _db;

        public RoleService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 🔹 Create Role
        public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Role name is required");

            if (string.IsNullOrWhiteSpace(dto.Code))
                throw new Exception("Role code is required");

            var code = dto.Code.Trim().ToUpper();

            var exists = await _db.Roles.AnyAsync(r => r.Code == code);
            if (exists)
                throw new Exception("Role code already exists");

            var role = new Role
            {
                Name = dto.Name,
                Code = code,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            return new RoleResponseDto
            {
                RoleId = role.RoleId,
                Name = role.Name,
                Code = role.Code,
                IsActive = role.IsActive
            };
        }

        // 🔹 Get All Roles
        public async Task<List<RoleResponseDto>> GetAllAsync()
        {
            return await _db.Roles
                .Select(r => new RoleResponseDto
                {
                    RoleId = r.RoleId,
                    Name = r.Name,
                    Code = r.Code,
                    IsActive = r.IsActive
                })
                .ToListAsync();
        }

        // 🔹 Get by Id
        public async Task<RoleResponseDto?> GetByIdAsync(int roleId)
        {
            return await _db.Roles
                .Where(r => r.RoleId == roleId)
                .Select(r => new RoleResponseDto
                {
                    RoleId = r.RoleId,
                    Name = r.Name,
                    Code = r.Code,
                    IsActive = r.IsActive
                })
                .FirstOrDefaultAsync();
        }

        // 🔹 Update Role
        public async Task<RoleResponseDto> UpdateAsync(int roleId, UpdateRoleDto dto)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role == null)
                throw new Exception("Role not found");

            role.Name = dto.Name;
            role.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();

            return new RoleResponseDto
            {
                RoleId = role.RoleId,
                Name = role.Name,
                Code = role.Code,
                IsActive = role.IsActive
            };
        }

        // 🔹 Delete Role (Soft Delete Alternative)
        public async Task<bool> DeleteAsync(int roleId)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);

            if (role == null)
                throw new Exception("Role not found");

            // 🔴 Instead of hard delete → deactivate
            role.IsActive = false;

            await _db.SaveChangesAsync();

            return true;
        }
    }
}