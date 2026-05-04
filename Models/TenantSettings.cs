namespace task_generator.Models
{
    public class TenantSettings
    {
        public int TenantSettingsId { get; set; }

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public string SettingsJson { get; set; } = "{}";
    }
}
