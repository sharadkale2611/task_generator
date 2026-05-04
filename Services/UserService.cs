using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Models;

namespace task_generator.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        // 🔹 Create User
        public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email is required");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Password is required");

            // 🔹 Validate Tenant
            var tenant = await _db.Tenants
                .FirstOrDefaultAsync(t => t.TenantId == dto.TenantId && !t.IsDeleted);

            if (tenant == null)
                throw new Exception("Invalid tenant");

            // 🔹 Validate Role
            var role = await _db.Roles
                .FirstOrDefaultAsync(r => r.RoleId == dto.RoleId && r.IsActive);

            if (role == null)
                throw new Exception("Invalid role");

            // 🔹 Unique email per tenant
            var exists = await _db.Users
                .AnyAsync(u => u.TenantId == dto.TenantId && u.Email == dto.Email);

            if (exists)
                throw new Exception("User already exists in this tenant");

            // 🔹 Hash password (simple)
            var passwordHash = Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(dto.Password))
            );

            var user = new User
            {
                TenantId = dto.TenantId,
                RoleId = dto.RoleId,
                Name = dto.Name,
                Email = dto.Email.ToLower(),
                Phone = dto.Phone,
                PasswordHash = passwordHash,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return new UserResponseDto
            {
                UserId = user.UserId,
                TenantId = tenant.TenantId,
                TenantName = tenant.Name,
                RoleId = role.RoleId,
                RoleName = role.Name,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }

        // 🔹 Get Users by Tenant
        public async Task<List<UserResponseDto>> GetByTenantAsync(int tenantId)
        {
            return await _db.Users
                .Where(u => u.TenantId == tenantId && !u.IsDeleted)
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    TenantId = u.TenantId,
                    TenantName = u.Tenant.Name,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    IsActive = u.IsActive
                })
                .ToListAsync();
        }

        // 🔹 Get by Id
        public async Task<UserResponseDto?> GetByIdAsync(int userId)
        {
            return await _db.Users
                .Where(u => u.UserId == userId && !u.IsDeleted)
                .Select(u => new UserResponseDto
                {
                    UserId = u.UserId,
                    TenantId = u.TenantId,
                    TenantName = u.Tenant.Name,
                    RoleId = u.RoleId,
                    RoleName = u.Role.Name,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    IsActive = u.IsActive
                })
                .FirstOrDefaultAsync();
        }

        // 🔹 Update User
        public async Task<UserResponseDto> UpdateAsync(int userId, UpdateUserDto dto)
        {
            var user = await _db.Users
                .Include(u => u.Role)
                .Include(u => u.Tenant)
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);

            if (user == null)
                throw new Exception("User not found");

            var role = await _db.Roles
                .FirstOrDefaultAsync(r => r.RoleId == dto.RoleId && r.IsActive);

            if (role == null)
                throw new Exception("Invalid role");

            user.Name = dto.Name;
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return new UserResponseDto
            {
                UserId = user.UserId,
                TenantId = user.TenantId,
                TenantName = user.Tenant.Name,
                RoleId = role.RoleId,
                RoleName = role.Name,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                IsActive = user.IsActive
            };
        }

        // 🔹 Soft Delete
        public async Task<bool> DeleteAsync(int userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                throw new Exception("User not found");

            user.IsDeleted = true;
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return true;
        }
    }
}