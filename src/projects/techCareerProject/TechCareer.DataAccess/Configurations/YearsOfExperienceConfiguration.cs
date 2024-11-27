using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechCareer.DataAccess.Configurations;

public class YearsOfExperienceConfiguration : IEntityTypeConfiguration<YearsOfExperience>
{
    public void Configure(EntityTypeBuilder<YearsOfExperience> builder)
    {
        builder.ToTable("YearsOfExperience").HasKey(y => y.Id);

        builder.Property(y => y.Id).HasColumnName("Id").IsRequired();
        builder.Property(y => y.Name)
            .HasColumnName("Name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(y => y.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(y => y.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(y => y.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(y => !y.DeletedDate.HasValue);

        builder.HasMany(y => y.Jobs)
            .WithOne(j => j.YearsOfExperienceNavigation)
            .HasForeignKey(j => j.YearsOfExperience);
    }
}
