using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechCareer.DataAccess.Configurations;

public class TypOfWorkConfiguration : IEntityTypeConfiguration<TypOfWork>
{
    public void Configure(EntityTypeBuilder<TypOfWork> builder)
    {
        builder.ToTable("TypOfWork").HasKey(t => t.Id);

        builder.Property(t => t.Id).HasColumnName("Id").IsRequired();
        builder.Property(t => t.Name)
            .HasColumnName("Name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(t => t.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(t => !t.DeletedDate.HasValue);

        builder.HasMany(t => t.Jobs)
            .WithOne(j => j.TypeOfWorkNavigation)
            .HasForeignKey(j => j.TypeOfWork);
    }
}
