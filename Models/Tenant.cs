using task_generator.Helpers;

namespace task_generator.Models
{
    public class Tenant
    {
        public int TenantId { get; set; }

        // Basic Info
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public TenantType Type { get; set; }

        // Contact
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        // Status
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<TenantAddress> Addresses { get; set; } = new List<TenantAddress>();
        public ICollection<TenantSubscription> Subscriptions { get; set; } = new List<TenantSubscription>();
        public TenantUsage? Usage { get; set; }
        public TenantSettings? Settings { get; set; }
    }
}
