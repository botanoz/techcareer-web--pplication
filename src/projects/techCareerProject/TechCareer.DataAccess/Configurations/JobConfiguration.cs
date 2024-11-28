using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechCareer.DataAccess.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("Jobs").HasKey(j => j.Id);

        builder.Property(j => j.Id).HasColumnName("Id").IsRequired();
        builder.Property(j => j.Title)
            .HasColumnName("Title")
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(j => j.Skills)
            .HasColumnName("Skills")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(j => j.StartDate)
            .HasColumnName("StartDate")
            .HasColumnType("datetime")
            .IsRequired();
        builder.Property(j => j.Content)
            .HasColumnName("Content")
            .IsRequired();
        builder.Property(j => j.Description)
            .HasColumnName("Description")
            .IsRequired();

        builder.HasOne(j => j.Company)
            .WithMany(c => c.Jobs)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Jobs_Companies");

        builder.HasOne(j => j.TypeOfWorkNavigation)
            .WithMany(t => t.Jobs)
            .HasForeignKey(j => j.TypeOfWork)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Jobs_TypOfWork");

        builder.HasOne(j => j.WorkPlaceNavigation)
            .WithMany(w => w.Jobs)
            .HasForeignKey(j => j.WorkPlace)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Jobs_WorkPlace");

        builder.HasOne(j => j.YearsOfExperienceNavigation)
            .WithMany(y => y.Jobs)
            .HasForeignKey(j => j.YearsOfExperience)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Jobs_YearsOfExperience");
    }
}
