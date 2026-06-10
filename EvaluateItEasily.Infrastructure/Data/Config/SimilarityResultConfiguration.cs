using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class SimilarityResultConfiguration : IEntityTypeConfiguration<SimilarityResult>
    {
        public void Configure(EntityTypeBuilder<SimilarityResult> builder)
        {
            builder.ToTable("SimilarityResults").HasKey(x => x.Id);


            builder.Property(x => x.SimilarityScore)
                .IsRequired()
                .HasColumnType("float");

            builder.Property(x => x.Rank).IsRequired();

            builder.HasIndex(x => new { x.EvaluationId, x.Rank }).IsUnique();

            builder.HasOne(x => x.HistoricalProject)
                .WithMany(y => y.SimilarityResults)
                .HasForeignKey(y => y.HistoricalProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
