using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class SubmissionPeriodConfiguration : IEntityTypeConfiguration<SubmissionPeriod>
    {
        public void Configure(EntityTypeBuilder<SubmissionPeriod> builder)
        {
            builder.ToTable("SubmissionPeriods").HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.EndDate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // computed 
            builder.Ignore(x => x.IsOpen);

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
