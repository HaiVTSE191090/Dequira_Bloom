using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pv => pv.SKU)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(pv => pv.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(pv => pv.StockQuantity)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(pv => pv.IsActive)
            .HasDefaultValue(true);

        // Concurrency control
        builder.Property(pv => pv.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.Property(pv => pv.CreatedAt)
            .IsRequired();

        builder.Property(pv => pv.IsDeleted)
            .HasDefaultValue(false);

        // Unique constraints
        builder.HasIndex(pv => pv.SKU)
            .IsUnique()
            .HasDatabaseName("IX_ProductVariants_SKU");

        // Performance indexes
        builder.HasIndex(pv => pv.ProductId)
            .HasDatabaseName("IX_ProductVariants_ProductId");

        builder.HasIndex(pv => new { pv.ProductId, pv.IsActive })
            .HasDatabaseName("IX_ProductVariants_ProductId_IsActive");

        builder.HasIndex(pv => pv.IsDeleted)
            .HasDatabaseName("IX_ProductVariants_IsDeleted");

        // Relationship configured in ProductConfiguration
    }
}
