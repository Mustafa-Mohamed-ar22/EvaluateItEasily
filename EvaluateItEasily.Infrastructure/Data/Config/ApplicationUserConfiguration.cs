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

            builder.OwnsMany(x => x.RefreshTokens).ToTable("RefreshTokens").WithOwner().HasForeignKey("UserId");
        }
    }
    public class GroupInvitationConfiguration : IEntityTypeConfiguration<GroupInvitation>
    {
        public void Configure(EntityTypeBuilder<GroupInvitation> builder)
        {
            builder.HasKey(gi => gi.Id);
            builder.HasIndex(gi => new { gi.GroupId, gi.InvitedStudentId })
                .IsUnique();

            builder.Property(gi => gi.Status).IsRequired().HasConversion<string>().HasMaxLength(20);

            builder.Property(gi => gi.RespondedAt).IsRequired(false);

            builder.HasOne(gi => gi.Group).WithMany(g => g.Invitations).HasForeignKey(gi => gi.GroupId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gi => gi.InvitedStudent).WithMany().HasForeignKey(gi => gi.InvitedStudentId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gi => gi.CreatedBy).WithMany().HasForeignKey(gi => gi.CreatedById).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(gi => gi.UpdatedBy).WithMany().HasForeignKey(gi => gi.UpdatedById).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
