using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Categories;

namespace Infrastructure.Persistence.EntityConfigurations;

public class CategoryConfiguration:IEntityTypeConfiguration<Category> 
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("tbl_category");
        
        //primary key
        builder.HasKey(c => c.Id)
            .HasName("pk_ID_CATEGORY");
        
        //property id
        builder.Property(c => c.Id)
            .HasColumnName("ID")
            .IsRequired();
            
        builder.Property(c => c.Name) 
            .HasColumnName("NAME")
            .IsRequired()
            .HasMaxLength(150)
            .HasColumnType("varchar(150)");

        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("uq_tbl_category_NAME");
        
        builder.Property(c => c.Description)
            .HasColumnName("DESCRIPTION")
            .HasColumnType("VARCHAR(200)")
            .HasMaxLength(200)
            .HasConversion(
                v => string.IsNullOrWhiteSpace(v) ? null : v.Trim(),
                v => v
            );
    }
}