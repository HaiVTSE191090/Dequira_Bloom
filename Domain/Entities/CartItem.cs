using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation Properties
        public virtual Cart Cart { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ProductVariant? ProductVariant { get; set; }

        // Domain Methods
        /// <summary>
        /// Calculate cart item total
        /// </summary>
        public decimal GetTotal() => Quantity * UnitPrice;

        /// <summary>
        /// Update quantity with validation
        /// </summary>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(newQuantity));

            Quantity = newQuantity;
        }

        /// <summary>
        /// Increase quantity by amount
        /// </summary>
        public void IncreaseQuantity(int amount = 1)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0", nameof(amount));

            Quantity += amount;
        }

        /// <summary>
        /// Decrease quantity by amount
        /// </summary>
        public void DecreaseQuantity(int amount = 1)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0", nameof(amount));

            if (Quantity - amount < 1)
                throw new InvalidOperationException("Resulting quantity would be less than 1");

            Quantity -= amount;
        }
    }
}

