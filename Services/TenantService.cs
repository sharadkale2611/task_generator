using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;
using task_generator.Helpers;
using task_generator.Models;

namespace task_generator.Services
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _db;

        public TenantService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Tenant> CreateTenantAsync(CreateTenantDto dto)
        {
            // 🔥 VALIDATION
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Tenant name is required");

            if (string.IsNullOrWhiteSpace(dto.Slug))
                throw new Exception("Slug is required");

            if (dto.SubscriptionPlanId <= 0)
                throw new Exception("Invalid subscription plan");

            if (!Enum.IsDefined(typeof(TenantType), dto.Type))
                throw new Exception("Invalid tenant type");

            if (string.IsNullOrWhiteSpace(dto.AddressLine1))
                throw new Exception("Address is required");

            var slug = dto.Slug.Trim().ToLower();

            bool slugExists = await _db.Tenants
                .AnyAsync(t => t.Slug == slug);

            if (slugExists)
                throw new Exception("Slug already exists");

            var plan = await _db.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.SubscriptionPlanId == dto.SubscriptionPlanId && p.IsActive);

            if (plan == null)
                throw new Exception("Invalid subscription plan");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ Create Tenant
                var tenant = new Tenant
                {
                    Name = dto.Name,
                    Slug = slug,
                    Type = dto.Type,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _db.Tenants.AddAsync(tenant);

                // 🔥 SAVE HERE to generate TenantId
                await _db.SaveChangesAsync();

                // 2️⃣ Address
                var address = new TenantAddress
                {
                    TenantId = tenant.TenantId,
                    AddressLine1 = dto.AddressLine1,
                    City = dto.City,
                    State = dto.State,
                    Country = dto.Country,
                    IsPrimary = true
                };

                await _db.TenantAddresses.AddAsync(address);

                // 3️⃣ Subscription
                var subscription = new TenantSubscription
                {
                    TenantId = tenant.TenantId,
                    SubscriptionPlanId = dto.SubscriptionPlanId,
                    StartDate = DateTime.UtcNow,
                    IsTrial = true,
                    IsActive = true
                };

                await _db.TenantSubscriptions.AddAsync(subscription);

                // 4️⃣ Usage
                var usage = new TenantUsage
                {
                    TenantId = tenant.TenantId,
                    TotalStudents = 0,
                    ActiveStudents = 0,
                    AiCreditsUsed = 0,
                    AiCreditsLimit = plan.AiCreditsLimit,
                    LastUpdated = DateTime.UtcNow
                };

                await _db.TenantUsages.AddAsync(usage);

                // 5️⃣ Settings
                var settings = new TenantSettings
                {
                    TenantId = tenant.TenantId,
                    SettingsJson = "{}"
                };

                await _db.TenantSettings.AddAsync(settings);

                // 🔥 FINAL SAVE
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return tenant;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<TenantListDto>> GetTenantsAsync()
        {
            var tenants = await _db.Tenants
                .Where(t => !t.IsDeleted)
                .Select(t => new TenantListDto
                {
                    TenantId = t.TenantId,
                    Name = t.Name,
                    Slug = t.Slug,
                    Type = t.Type.ToString(),
                    Email = t.Email,
                    IsActive = t.IsActive,

                    TotalStudents = t.Usage != null ? t.Usage.TotalStudents : 0,
                    ActiveStudents = t.Usage != null ? t.Usage.ActiveStudents : 0,

                    PlanName = t.Subscriptions
                        .Where(s => s.IsActive)
                        .Select(s => s.SubscriptionPlan.Name)
                        .FirstOrDefault() ?? "N/A"
                })
                .OrderByDescending(t => t.TenantId)
                .ToListAsync();

            return tenants;
        }

        public async Task<TenantDetailDto?> GetTenantByIdAsync(int tenantId)
        {
            var tenant = await _db.Tenants
                .Where(t => t.TenantId == tenantId && !t.IsDeleted)
                .Select(t => new TenantDetailDto
                {
                    TenantId = t.TenantId,
                    Name = t.Name,
                    Slug = t.Slug,
                    Type = t.Type.ToString(),
                    Email = t.Email,
                    Phone = t.Phone,
                    IsActive = t.IsActive,

                    AddressLine1 = t.Addresses
                        .Where(a => a.IsPrimary)
                        .Select(a => a.AddressLine1)
                        .FirstOrDefault() ?? "",

                    City = t.Addresses
                        .Where(a => a.IsPrimary)
                        .Select(a => a.City)
                        .FirstOrDefault() ?? "",

                    State = t.Addresses
                        .Where(a => a.IsPrimary)
                        .Select(a => a.State)
                        .FirstOrDefault() ?? "",

                    Country = t.Addresses
                        .Where(a => a.IsPrimary)
                        .Select(a => a.Country)
                        .FirstOrDefault() ?? "",

                    PlanName = t.Subscriptions
                        .Where(s => s.IsActive)
                        .Select(s => s.SubscriptionPlan.Name)
                        .FirstOrDefault() ?? "N/A",

                    AiCreditsUsed = t.Usage != null ? t.Usage.AiCreditsUsed : 0,
                    AiCreditsLimit = t.Usage != null ? t.Usage.AiCreditsLimit : 0,

                    TotalStudents = t.Usage != null ? t.Usage.TotalStudents : 0,
                    ActiveStudents = t.Usage != null ? t.Usage.ActiveStudents : 0
                })
                .FirstOrDefaultAsync();

            return tenant;
        }

        // 🔹 Update Tenant
        public async Task<Tenant> UpdateAsync(int tenantId, TenantUpdateDto dto)
        {
            if (tenantId <= 0)
                throw new Exception("Invalid tenant id");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email is required");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var tenant = await _db.Tenants
                    .FirstOrDefaultAsync(t => t.TenantId == tenantId && !t.IsDeleted);

                if (tenant == null)
                    throw new Exception("Tenant not found");

                // 🔹 Update Tenant fields
                tenant.Name = dto.Name.Trim();
                tenant.Email = dto.Email.Trim();
                tenant.Phone = dto.Phone;
                tenant.IsActive = dto.IsActive;
                tenant.UpdatedAt = DateTime.UtcNow;

                // 🔹 Update Primary Address
                var address = await _db.TenantAddresses
                    .FirstOrDefaultAsync(a => a.TenantId == tenantId && a.IsPrimary);

                if (address == null)
                {
                    // Create if not exists (safe fallback)
                    address = new TenantAddress
                    {
                        TenantId = tenantId,
                        IsPrimary = true
                    };

                    await _db.TenantAddresses.AddAsync(address);
                }

                address.AddressLine1 = dto.AddressLine1;
                address.City = dto.City;
                address.State = dto.State;
                address.Country = dto.Country;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return tenant;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // 🔹 Soft Delete Tenant
        public async Task<bool> DeleteAsync(int tenantId)
        {
            if (tenantId <= 0)
                throw new Exception("Invalid tenant id");

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var tenant = await _db.Tenants
                    .FirstOrDefaultAsync(t => t.TenantId == tenantId && !t.IsDeleted);

                if (tenant == null)
                    throw new Exception("Tenant not found");

                // 🔴 Soft delete tenant
                tenant.IsDeleted = true;
                tenant.IsActive = false;
                tenant.UpdatedAt = DateTime.UtcNow;

                // 🔴 Deactivate active subscriptions
                var subscriptions = await _db.TenantSubscriptions
                    .Where(s => s.TenantId == tenantId && s.IsActive)
                    .ToListAsync();

                foreach (var sub in subscriptions)
                {
                    sub.IsActive = false;
                    sub.EndDate = DateTime.UtcNow;
                }

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
    }
}