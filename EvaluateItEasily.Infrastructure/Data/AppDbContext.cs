using EvaluateItEasily.Infrastructure.Data.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace EvaluateItEasily.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, 
        ICurrentUserService currentUserService) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly ICurrentUserService _currentUserService= currentUserService;

        public DbSet<Decision> Decisions { get; set; } 
        public DbSet<Group> Groups { get; set; } 
        public DbSet<GroupMember> GroupMembers { get; set; } 
        public DbSet<HistoricalProject> HistoricalProjects { get; set; } 
        public DbSet<Notification> Notifications { get; set; } 
        public DbSet<SimilarityResult> SimilarityResults { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<SupervisorAssignment> SupervisorAssignments { get; set; }
        public DbSet<GroupInvitation> GroupInvitations { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        public DbSet<SubmissionPeriod> SubmissionPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationUserConfiguration).Assembly);
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var trackedEntries = ChangeTracker.Entries<AuditableEntity>();
            var userId = _currentUserService.GetUserId();
            foreach (var entityEntry in trackedEntries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(x => x.CreatedById).CurrentValue = userId!;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(x => x.UpdatedById).CurrentValue = userId;
                    entityEntry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;

                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
