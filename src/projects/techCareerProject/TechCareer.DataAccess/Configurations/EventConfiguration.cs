using Core.Security.Entities;
using Core.Security.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechCareer.DataAccess.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events").HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(e => e.Title)
                .HasColumnName("Title")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasColumnName("Description")
                .HasMaxLength(1000);

            builder.Property(e => e.ImageUrl)
                .HasColumnName("ImageUrl")
                .HasMaxLength(250);

            builder.Property(e => e.StartDate)
                .HasColumnName("StartDate")
                .HasColumnType("datetime");

            builder.Property(e => e.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("datetime");

            builder.Property(e => e.ApplicationDeadline)
                .HasColumnName("ApplicationDeadline")
                .HasColumnType("datetime");

            builder.Property(e => e.ParticipationText)
                .HasColumnName("ParticipationText")
                .HasColumnType("nvarchar(max)");

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            builder.HasData(EventSeedData.GetSeedData());
        }
    }
}
