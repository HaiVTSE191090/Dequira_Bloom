using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{


    public class ProductVariant : BaseEntity, IConcurrencyControl
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Concurrency Control
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // Domain Methods
        public bool IsInStock(int requestedQuantity = 1)
            => StockQuantity >= requestedQuantity;

        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            if (!IsInStock(quantity))
                throw new InvalidOperationException($"Insufficient stock for variant '{Name}'. Available: {StockQuantity}, Requested: {quantity}");

            StockQuantity -= quantity;
        }

        public void RestoreStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            StockQuantity += quantity;
        }
    }
}
