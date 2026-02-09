using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.ToTable("Wishlists");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.IsDeleted)
            .HasDefaultValue(false);

        // Unique constraint - one product per user in wishlist
        builder.HasIndex(w => new { w.UserId, w.ProductId })
            .IsUnique()
            .HasDatabaseName("IX_Wishlists_UserId_ProductId");

        // Performance indexes
        builder.HasIndex(w => w.UserId)
            .HasDatabaseName("IX_Wishlists_UserId");

        builder.HasIndex(w => w.ProductId)
            .HasDatabaseName("IX_Wishlists_ProductId");

        builder.HasIndex(w => w.IsDeleted)
            .HasDatabaseName("IX_Wishlists_IsDeleted");

        // Relationships configured in User and Product configurations
    }
}
