using EvaluateItEasily.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class SupervisorAssignmentConfiguration : IEntityTypeConfiguration<SupervisorAssignment>
    {
        public void Configure(EntityTypeBuilder<SupervisorAssignment> builder)
        {
            builder.ToTable("SupervisorAssignments").HasKey(x=>x.Id);

            builder.Property(x => x.WorkloadNote)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.AssignedAt)
                .IsRequired();

           
            builder.HasOne(x => x.AssignedByUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedById)
                .OnDelete(DeleteBehavior.Restrict);

            
        }
    }
}
