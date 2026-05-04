using Microsoft.EntityFrameworkCore;
using task_generator.Data;
using task_generator.Dto;

namespace task_generator.Middlewares
{
    public class TenantSubscriptionMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantSubscriptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db, TenantContext tenantContext)
        {
            // 🔹 1. Get TenantId (from header / JWT / subdomain)
            if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader))
            {
                await _next(context);
                return;
            }

            if (!int.TryParse(tenantIdHeader, out int tenantId))
            {
                await _next(context);
                return;
            }

            tenantContext.TenantId = tenantId;

            // 🔹 2. Get active subscription
            var subscription = await db.TenantSubscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.IsActive);

            // 🔴 No subscription → block
            if (subscription == null)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("No active subscription");
                return;
            }

            // 🔹 3. Attach to context
            tenantContext.Subscription = subscription;

            await _next(context);
        }
    }
}
