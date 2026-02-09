using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Data;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.ToTable("ProductReviews");

        builder.HasKey(pr => pr.Id);

        builder.Property(pr => pr.Rating)
            .IsRequired();

        builder.Property(pr => pr.Title)
            .HasMaxLength(200);

        builder.Property(pr => pr.Comment)
            .HasMaxLength(1000);

        builder.Property(pr => pr.IsVerifiedPurchase)
            .HasDefaultValue(false);

        builder.Property(pr => pr.IsApproved)
            .HasDefaultValue(false);

        builder.Property(pr => pr.CreatedAt)
            .IsRequired();

        builder.Property(pr => pr.IsDeleted)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(pr => pr.ProductId)
            .HasDatabaseName("IX_ProductReviews_ProductId");

        builder.HasIndex(pr => pr.UserId)
            .HasDatabaseName("IX_ProductReviews_UserId");

        builder.HasIndex(pr => pr.OrderId)
            .HasDatabaseName("IX_ProductReviews_OrderId");

        builder.HasIndex(pr => new { pr.ProductId, pr.IsApproved })
            .HasDatabaseName("IX_ProductReviews_ProductId_IsApproved");

        builder.HasIndex(pr => new { pr.ProductId, pr.Rating })
            .HasDatabaseName("IX_ProductReviews_ProductId_Rating");

        builder.HasIndex(pr => pr.CreatedAt)
            .IsDescending()
            .HasDatabaseName("IX_ProductReviews_CreatedAt");

        builder.HasIndex(pr => pr.IsDeleted)
            .HasDatabaseName("IX_ProductReviews_IsDeleted");

        // Relationships
        builder.HasOne(pr => pr.Order)
            .WithMany()
            .HasForeignKey(pr => pr.OrderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relationships with Product and User configured in their configurations
    }
}
