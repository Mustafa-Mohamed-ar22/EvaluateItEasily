using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class HistoricalProjectConfiguration : IEntityTypeConfiguration<HistoricalProject>
    {
        public void Configure(EntityTypeBuilder<HistoricalProject> builder)
        {
            builder.ToTable("HistoricalProjects").HasKey(t => t.Id);

            builder.Property(x => x.Name)
                .IsRequired().HasColumnType("nvarchar(MAX)");

            builder.Property(x => x.Abstract)
                .IsRequired()
                .HasColumnType("nvarchar(MAX)");

            builder.Property(x => x.GroupName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.AcademicYear).IsRequired().HasMaxLength(350);             
            builder.Property(x => x.ArchivedAt).IsRequired();
            builder.Property(x => x.ProposalId).IsRequired(false);

        }
    }

}
