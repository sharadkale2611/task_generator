using task_generator.Models;

namespace task_generator.Dto
{
    public class TenantContext
    {   
        public int TenantId { get; set; }
        public TenantSubscription? Subscription { get; set; }
    }
}
