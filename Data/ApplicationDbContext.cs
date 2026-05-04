using Microsoft.EntityFrameworkCore;
using task_generator.Models;

namespace task_generator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TechStack> TechStacks { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<ProjectDomain> ProjectDomains { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<EpicTechStack> EpicTechStacks { get; set; }
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<WorkItemAssignment> WorkItemAssignments { get; set; }

        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmissionFile> SubmissionFiles { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }


        //---------- Onboarding details ----------

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<TenantAddress> TenantAddresses { get; set; }
        public DbSet<TenantSubscription> TenantSubscriptions { get; set; }
        public DbSet<TenantUsage> TenantUsages { get; set; }
        public DbSet<TenantSettings> TenantSettings { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EpicTechStack>()
                .HasKey(x => new { x.EpicId, x.TechStackId });

            modelBuilder.Entity<EpicTechStack>()
                .HasOne(x => x.Epic)
                .WithMany(e => e.EpicTechStacks)
                .HasForeignKey(x => x.EpicId);

            modelBuilder.Entity<EpicTechStack>()
                .HasOne(x => x.TechStack)
                .WithMany(t => t.EpicTechStacks)
                .HasForeignKey(x => x.TechStackId);


            // Unique Slug
            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.Slug)
                .IsUnique();

            // Tenant → Address (1:M)
            modelBuilder.Entity<TenantAddress>()
                .HasOne(a => a.Tenant)
                .WithMany(t => t.Addresses)
                .HasForeignKey(a => a.TenantId);

            // Tenant → Subscription (1:M)
            modelBuilder.Entity<TenantSubscription>()
                .HasOne(s => s.Tenant)
                .WithMany(t => t.Subscriptions)
                .HasForeignKey(s => s.TenantId);

            // Tenant → Usage (1:1)
            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.Usage)
                .WithOne(u => u.Tenant)
                .HasForeignKey<TenantUsage>(u => u.TenantId);

            // Tenant → Settings (1:1)
            modelBuilder.Entity<Tenant>()
                .HasOne(t => t.Settings)
                .WithOne(s => s.Tenant)
                .HasForeignKey<TenantSettings>(s => s.TenantId);

            modelBuilder.Entity<SubscriptionPlan>()
                .HasKey(p => p.SubscriptionPlanId);

            modelBuilder.Entity<SubscriptionPlan>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Code)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.TenantId, u.Email })
                .IsUnique();

            modelBuilder.Entity<WorkItemAssignment>()
                .HasOne(a => a.WorkItem)
                .WithMany()
                .HasForeignKey(a => a.WorkItemId);

            modelBuilder.Entity<WorkItemAssignment>()
                .HasOne(a => a.AssignedToUser)
                .WithMany()
                .HasForeignKey(a => a.AssignedToUserId);

        }

    }
}
