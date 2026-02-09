using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.SessionId)
            .HasMaxLength(200);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("IX_Carts_UserId");

        builder.HasIndex(c => c.SessionId)
            .HasDatabaseName("IX_Carts_SessionId");

        builder.HasIndex(c => c.IsDeleted)
            .HasDatabaseName("IX_Carts_IsDeleted");

        // Relationships
        builder.HasMany(c => c.Items)
            .WithOne(ci => ci.Cart)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship with User configured in UserConfiguration
    }
}
