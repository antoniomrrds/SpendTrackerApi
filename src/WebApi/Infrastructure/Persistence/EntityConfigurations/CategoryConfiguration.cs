using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Domain.Categories;

namespace WebApi.Infrastructure.Persistence.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("tbl_category");

        builder.HasKey(c => c.Id).HasName("pk_ID_CATEGORY");

        builder.Property(c => c.Id).HasColumnName("ID").IsRequired();

        builder
            .Property(c => c.Name)
            .HasColumnName("NAME")
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)");

        builder.HasIndex(e => e.Name).IsUnique().HasDatabaseName("uq_tbl_category_NAME");

        builder
            .Property(c => c.Description)
            .HasColumnName("DESCRIPTION")
            .HasColumnType("VARCHAR(200)")
            .HasMaxLength(200)
            .HasConversion(v => string.IsNullOrWhiteSpace(v) ? null : v.Trim(), v => v);
    }
}
