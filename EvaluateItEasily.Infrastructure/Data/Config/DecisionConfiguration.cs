using EvaluateItEasily.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class DecisionConfiguration : IEntityTypeConfiguration<Decision>
    {
        public void Configure(EntityTypeBuilder<Decision> builder)
        {
            builder.ToTable("Decisions").HasKey(x => x.Id); 

            builder.Property(x => x.DecisionType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(x => x.FeedbackComment)
                .IsRequired()
                .HasColumnType("nvarchar(MAX)");

            builder.Property(x => x.DecidedAt)
                .IsRequired();

            
            builder.HasOne(x => x.DecidedByUser).WithMany().HasForeignKey(x => x.DecidedById).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
