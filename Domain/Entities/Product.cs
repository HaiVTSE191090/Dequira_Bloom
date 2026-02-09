using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public decimal? CostPerItem { get; set; }
        public int StockQuantity { get; set; } = 0;
        public int LowStockThreshold { get; set; } = 5;
        public decimal? Weight { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public int ViewCount { get; set; } = 0;
        public int SoldCount { get; set; } = 0;

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();


        // Navigation Properties
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public virtual ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

        // Domain Methods
        public bool IsInStock(int requestedQuantity = 1)
            => StockQuantity >= requestedQuantity;

        public bool IsLowStock()
            => StockQuantity <= LowStockThreshold && StockQuantity > 0;

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            if (!IsInStock(quantity))
                throw new InvalidOperationException($"Insufficient stock. Available: {StockQuantity}, Requested: {quantity}");

            StockQuantity -= quantity;
            SoldCount += quantity;
        }

        public void RestoreStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            StockQuantity += quantity;
            SoldCount = Math.Max(0, SoldCount - quantity);
        }
    }
}
