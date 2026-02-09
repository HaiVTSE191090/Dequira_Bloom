using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Quantity)
            .IsRequired();

        builder.Property(ci => ci.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(ci => ci.CreatedAt)
            .IsRequired();

        builder.Property(ci => ci.IsDeleted)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(ci => ci.CartId)
            .HasDatabaseName("IX_CartItems_CartId");

        builder.HasIndex(ci => ci.ProductId)
            .HasDatabaseName("IX_CartItems_ProductId");

        builder.HasIndex(ci => ci.ProductVariantId)
            .HasDatabaseName("IX_CartItems_ProductVariantId");

        builder.HasIndex(ci => ci.IsDeleted)
            .HasDatabaseName("IX_CartItems_IsDeleted");

        // Relationships
        builder.HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ci => ci.ProductVariant)
            .WithMany()
            .HasForeignKey(ci => ci.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Cart configured in CartConfiguration
    }
}
