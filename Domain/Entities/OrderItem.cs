using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{


    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid? ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ProductVariant? ProductVariant { get; set; }

        // Domain Methods
        /// <summary>
        /// Calculate item subtotal: SubTotal = Qi × Pi
        /// </summary>
        public void CalculateSubTotal()
        {
            if (Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(Quantity));

            if (UnitPrice < 0)
                throw new ArgumentException("Unit price cannot be negative", nameof(UnitPrice));

            SubTotal = Quantity * UnitPrice;
        }

        /// <summary>
        /// Update quantity and recalculate
        /// </summary>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(newQuantity));

            Quantity = newQuantity;
            CalculateSubTotal();
        }
    }
}
