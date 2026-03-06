using EvaluateItEasily.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaluateItEasily.Infrastructure.Data.Config
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications").HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Message).IsRequired().HasColumnType("nvarchar(MAX)");

            builder.Property(x => x.IsRead).HasDefaultValue(false);

            builder.Property(x => x.Type).IsRequired().HasConversion<string>().HasMaxLength(50);

            builder.Property(x => x.CreatedAt).IsRequired();

        }
    }
}
