using EvaluateItEasily.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedOn).IsRequired();


            builder.HasOne(u => u.LeadingGroup).WithOne(g => g.Leader).HasForeignKey<Group>(g => g.LeaderId);
            builder.HasMany(x=>x.GroupMemberships).WithOne(y=>y.Student).HasForeignKey(y=>y.StudentId);
            builder.HasMany(x=>x.Notifications).WithOne(y=>y.User).HasForeignKey(y=>y.UserId);
            builder.HasMany(x=>x.SupervisedProjects).WithOne(x=>x.Supervisor).HasForeignKey(y=>y.SupervisorId);

            builder.OwnsMany(x => x.RefreshTokens)
                         .ToTable("RefreshTokens")
                         .WithOwner()
                         .HasForeignKey("UserId");
        }
    }
}
