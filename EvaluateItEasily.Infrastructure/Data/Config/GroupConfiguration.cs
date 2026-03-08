using EvaluateItEasily.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("Groups").HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.LeaderId).IsRequired();

            
            builder.HasMany(x=>x.Members).WithOne(y=>y.Group).HasForeignKey(y=>y.GroupId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.Proposal).WithOne(y=>y.Group).HasForeignKey<Proposal>(y=>y.GroupId);
            builder.HasMany(x=>x.Members).WithOne(y=>y.Group).HasForeignKey(y=>y.GroupId);

            // logging ya negm
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
