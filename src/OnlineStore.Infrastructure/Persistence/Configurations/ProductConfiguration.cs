using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.HasIndex(p => p.Name)
            .IsUnique();
            
        builder.Property(p => p.Description)
            .HasMaxLength(1000);
            
        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
            
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);
            
        builder.Property(p => p.StockQuantity)
            .IsRequired();
            
        builder.HasCheckConstraint("CK_Product_Price", "Price > 0");
        builder.HasCheckConstraint("CK_Product_StockQuantity", "StockQuantity >= 0");
        
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
