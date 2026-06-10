using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.ToTable("GroupMembers").HasKey(x => x.Id);


            builder.HasIndex(x => new { x.GroupId, x.StudentId })
                .IsUnique();

            builder.Property(x => x.IsLeader)
                .HasDefaultValue(false);

            builder.Property(x => x.JoinedAt)
                .IsRequired();

            // Audit
            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedBy)
                .WithMany()
                .HasForeignKey(x => x.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
