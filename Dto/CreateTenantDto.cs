using task_generator.Helpers;

namespace task_generator.Dto
{
    public class CreateTenantDto
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public TenantType Type { get; set; }

        public string Email { get; set; } = null!;
        public string? Phone { get; set; }

        // Address
        public string AddressLine1 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;

        // Subscription
        public int SubscriptionPlanId { get; set; }
    }
}
