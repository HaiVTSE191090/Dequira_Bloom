using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.ToTable("ShippingAddresses");

        builder.HasKey(sa => sa.Id);

        builder.Property(sa => sa.RecipientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sa => sa.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(sa => sa.AddressLine1)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(sa => sa.AddressLine2)
            .HasMaxLength(300);

        builder.Property(sa => sa.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(sa => sa.District)
            .HasMaxLength(100);

        builder.Property(sa => sa.Ward)
            .HasMaxLength(100);

        builder.Property(sa => sa.PostalCode)
            .HasMaxLength(20);

        builder.Property(sa => sa.CreatedAt)
            .IsRequired();

        builder.Property(sa => sa.IsDeleted)
            .HasDefaultValue(false);

        // Unique constraint - one shipping address per order
        builder.HasIndex(sa => sa.OrderId)
            .IsUnique()
            .HasDatabaseName("IX_ShippingAddresses_OrderId");

        builder.HasIndex(sa => sa.IsDeleted)
            .HasDatabaseName("IX_ShippingAddresses_IsDeleted");

        // Relationship configured in OrderConfiguration
    }
}
