using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Description)
            .HasMaxLength(300);

        builder.Property(c => c.DiscountType)
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion<string>();

        builder.Property(c => c.DiscountValue)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.MinimumOrderAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.MaxDiscountAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.UsedCount)
            .HasDefaultValue(0);

        builder.Property(c => c.StartDate)
            .IsRequired();

        builder.Property(c => c.EndDate)
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.IsDeleted)
            .HasDefaultValue(false);

        // Unique constraints
        builder.HasIndex(c => c.Code)
            .IsUnique()
            .HasDatabaseName("IX_Coupons_Code");

        // Performance indexes
        builder.HasIndex(c => new { c.IsActive, c.StartDate, c.EndDate })
            .HasDatabaseName("IX_Coupons_IsActive_Dates");

        builder.HasIndex(c => c.IsDeleted)
            .HasDatabaseName("IX_Coupons_IsDeleted");

        // Relationships
        builder.HasMany(c => c.Usages)
            .WithOne(cu => cu.Coupon)
            .HasForeignKey(cu => cu.CouponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
