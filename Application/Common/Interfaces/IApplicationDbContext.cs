using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        // Users & Auth
        DbSet<User> Users { get; }
        DbSet<RefreshToken> RefreshTokens { get; }

        // Products
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<ProductCategory> ProductCategories { get; }
        DbSet<ProductImage> ProductImages { get; }
        DbSet<ProductVariant> ProductVariants { get; }
        DbSet<ProductReview> ProductReviews { get; }

        // Shopping
        DbSet<Cart> Carts { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<Wishlist> Wishlists { get; }

        // Orders
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<ShippingAddress> ShippingAddresses { get; }

        // Coupons
        DbSet<Coupon> Coupons { get; }
        DbSet<CouponUsage> CouponUsages { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
