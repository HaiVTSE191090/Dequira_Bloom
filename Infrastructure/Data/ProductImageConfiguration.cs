using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(pi => pi.Id);

        builder.Property(pi => pi.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(pi => pi.AltText)
            .HasMaxLength(200);

        builder.Property(pi => pi.DisplayOrder)
            .HasDefaultValue(0);

        builder.Property(pi => pi.IsThumbnail)
            .HasDefaultValue(false);

        builder.Property(pi => pi.CreatedAt)
            .IsRequired();

        builder.Property(pi => pi.IsDeleted)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(pi => pi.ProductId)
            .HasDatabaseName("IX_ProductImages_ProductId");

        builder.HasIndex(pi => new { pi.ProductId, pi.DisplayOrder })
            .HasDatabaseName("IX_ProductImages_ProductId_DisplayOrder");

        builder.HasIndex(pi => new { pi.ProductId, pi.IsThumbnail })
            .HasDatabaseName("IX_ProductImages_ProductId_IsThumbnail");

        builder.HasIndex(pi => pi.IsDeleted)
            .HasDatabaseName("IX_ProductImages_IsDeleted");

        // Relationship configured in ProductConfiguration
    }
}
