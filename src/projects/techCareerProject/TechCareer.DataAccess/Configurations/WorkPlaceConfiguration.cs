using Core.Security.Entities;
using Core.Security.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechCareer.DataAccess.Configurations;

public class WorkPlaceConfiguration : IEntityTypeConfiguration<WorkPlace>
{
    public void Configure(EntityTypeBuilder<WorkPlace> builder)
    {
        builder.ToTable("WorkPlace").HasKey(w => w.Id);

        builder.Property(w => w.Id).HasColumnName("Id").IsRequired();
        builder.Property(w => w.Name)
            .HasColumnName("Name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(w => w.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(w => w.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(w => w.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(w => !w.DeletedDate.HasValue);

        builder.HasMany(w => w.Jobs)
            .WithOne(j => j.WorkPlaceNavigation)
            .HasForeignKey(j => j.WorkPlace);
        builder.HasData(WorkPlaceSeedData.GetSeedData());
    }
}
