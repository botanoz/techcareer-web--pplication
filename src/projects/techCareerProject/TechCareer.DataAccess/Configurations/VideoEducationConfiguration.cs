using Core.Security.Entities;
using Core.Security.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechCareer.DataAccess.Configurations;

public class VideoEducationConfiguration : IEntityTypeConfiguration<VideoEducation>
{
    public void Configure(EntityTypeBuilder<VideoEducation> builder)
    {
        builder.ToTable("VideoEducations").HasKey(ve => ve.Id);

        builder.Property(ve => ve.Id).HasColumnName("Id").IsRequired();
        builder.Property(ve => ve.Title)
            .HasColumnName("Title")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(ve => ve.Description)
            .HasColumnName("Description")
            .HasMaxLength(250);
        builder.Property(ve => ve.ImageUrl)
            .HasColumnName("ImageUrl")
            .HasMaxLength(250);
        builder.Property(ve => ve.TotalHour).HasColumnName("TotalHour").IsRequired();
        builder.Property(ve => ve.IsCertified).HasColumnName("IsCertified").IsRequired();
        builder.Property(ve => ve.Level).HasColumnName("Level").IsRequired();
        builder.Property(ve => ve.ProgrammingLanguage)
            .HasColumnName("ProgrammingLanguage")
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(ve => ve.Instructor)
            .WithMany(i => i.VideoEducations)
            .HasForeignKey(ve => ve.InstructorId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasData(VideoEducationSeedData.GetSeedData());
    }
}
