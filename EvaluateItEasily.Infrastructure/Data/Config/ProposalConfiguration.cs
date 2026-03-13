using EvaluateItEasily.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class ProposalConfiguration : IEntityTypeConfiguration<Proposal>
    {
        public void Configure(EntityTypeBuilder<Proposal> builder)
        {
            builder.ToTable("Proposals").HasKey(x => x.Id);


            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);

            builder.Property(p => p.Abstract).IsRequired().HasColumnType("nvarchar(MAX)");

            builder.Property(p => p.ProposalFileUrl).IsRequired().HasMaxLength(500);

            builder.Property(p => p.Status).IsRequired().HasConversion<string>().HasMaxLength(30);

            builder.Property(p => p.SubmittedAt).IsRequired();

            builder.Property(x => x.FileName).IsRequired().HasMaxLength(255);

            builder.ToTable("Proposals", t =>
            {
                t.HasCheckConstraint(
                    "CK_Proposals_FileName_Valid",
                    "FileName NOT LIKE '%[^A-Za-z0-9_ .]%' AND FileName NOT LIKE REPLICATE('.', LEN(FileName))"
                );
            });

            builder.Property(x => x.FileExtension).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ContentType).IsRequired().HasMaxLength(60);


            builder.HasOne(x => x.Evaluation).WithOne(y => y.Proposal).HasForeignKey<Evaluation>(y => y.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Decision)
                .WithOne(y => y.Proposal)
                .HasForeignKey<Decision>(y => y.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.SupervisorAssignment)
                .WithOne(y => y.Proposal)
                .HasForeignKey<SupervisorAssignment>(y => y.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.HistoricalProject)
                .WithOne(y =>y.Proposal)
                .HasForeignKey<HistoricalProject>(y => y.ProposalId)
                .OnDelete(DeleteBehavior.Restrict);  
        }
    }

}
