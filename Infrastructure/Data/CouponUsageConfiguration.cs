using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class CouponUsageConfiguration : IEntityTypeConfiguration<CouponUsage>
{
    public void Configure(EntityTypeBuilder<CouponUsage> builder)
    {
        builder.ToTable("CouponUsages");

        builder.HasKey(cu => cu.Id);

        builder.Property(cu => cu.UsedAt)
            .IsRequired();

        builder.Property(cu => cu.CreatedAt)
            .IsRequired();

        builder.Property(cu => cu.IsDeleted)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(cu => cu.CouponId)
            .HasDatabaseName("IX_CouponUsages_CouponId");

        builder.HasIndex(cu => cu.UserId)
            .HasDatabaseName("IX_CouponUsages_UserId");

        builder.HasIndex(cu => cu.OrderId)
            .HasDatabaseName("IX_CouponUsages_OrderId");

        builder.HasIndex(cu => new { cu.CouponId, cu.UserId })
            .HasDatabaseName("IX_CouponUsages_CouponId_UserId");

        builder.HasIndex(cu => cu.IsDeleted)
            .HasDatabaseName("IX_CouponUsages_IsDeleted");

        // Relationships
        builder.HasOne(cu => cu.User)
            .WithMany()
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationships with Coupon and Order configured in their configurations
    }
}
