using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Categories;
using Domain.Expenses;

namespace Infrastructure.Persistence.EntityConfigurations;

public class ExpenseConfiguration: IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("tbl_expense");
        
        //primary id
        builder.HasKey(c => c.Id)
            .HasName("pk_ID_EXPENSE");

        builder.Property(c => c.Description)
            .HasColumnName("DESCRIPTION")
            .HasMaxLength(500)
            .HasColumnType("VARCHAR(500)")
            .IsRequired(false);

        builder.Property(e => e.Date)
            .HasColumnName("DATE")
            .HasColumnType("DATETIME")
            .IsRequired();

        builder.Property(c => c.Amount)
            .HasColumnName("AMOUNT")
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(e => e.IdCategory)
            .HasColumnName("ID_CATEGORY")
            .IsRequired();
        
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(e => e.IdCategory)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_tbl_expense_tbl_category");
        
        builder.HasIndex(e => e.IdCategory)
            .HasDatabaseName("idx_tbl_expense_ID_CATEGORY");

        builder.HasIndex(e => e.Date)
            .HasDatabaseName("idx_tbl_expense_EXPENSE_DATE");
    }
}