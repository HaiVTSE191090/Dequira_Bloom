using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        // Composite primary key
        builder.HasKey(pc => new { pc.ProductId, pc.CategoryId });

        // Indexes for query performance
        builder.HasIndex(pc => pc.ProductId)
            .HasDatabaseName("IX_ProductCategories_ProductId");

        builder.HasIndex(pc => pc.CategoryId)
            .HasDatabaseName("IX_ProductCategories_CategoryId");

        // Relationships configured in Product and Category configurations
    }
}
