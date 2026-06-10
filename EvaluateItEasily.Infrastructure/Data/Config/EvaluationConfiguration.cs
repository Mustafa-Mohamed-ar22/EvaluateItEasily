using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class EvaluationConfiguration : IEntityTypeConfiguration<Evaluation>
    {
        public void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.ToTable("Evaluations").HasKey(x=>x.Id);

            builder.Property(x => x.AIStatus).IsRequired().HasConversion<string>().HasMaxLength(30);

            builder.Property(x => x.MaxSimilarityScore).IsRequired().HasColumnType("float");

            builder.Property(x => x.EvaluatedAt).IsRequired();

           builder.HasOne(x => x.EvaluatedByUser)
                .WithMany()
                .HasForeignKey(x => x.EvaluatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SimilarityResults)
                .WithOne(y => y.Evaluation)
                .HasForeignKey(y => y.EvaluationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
