namespace task_generator.Models
{
    public class TenantAddress
    {
        public int TenantAddressId { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }

        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? Pincode { get; set; }

        public bool IsPrimary { get; set; } = false;
    }
}
