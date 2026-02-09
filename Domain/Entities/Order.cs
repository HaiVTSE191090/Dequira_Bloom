using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; }
        public string? Note { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public virtual ShippingAddress? ShippingAddress { get; set; }
        public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();
        // Domain Methods - Financial Calculations
        /// <summary>
        /// Calculate Order Total: T_order = Σ(Qi × Pi) + F_ship - D_coupon
        /// </summary>
        public void CalculateTotals()
        {
            // Σ(Qi × Pi) - Sum of all item subtotals
            SubTotal = Items.Sum(item => item.SubTotal);

            // T_order = SubTotal + F_ship - D_coupon
            TotalAmount = SubTotal + ShippingFee - DiscountAmount;

            // Ensure total is never negative
            if (TotalAmount < 0)
                TotalAmount = 0;
        }

        /// <summary>
        /// Apply coupon discount to order
        /// </summary>
        public void ApplyDiscount(decimal discountAmount)
        {
            if (discountAmount < 0)
                throw new ArgumentException("Discount amount cannot be negative", nameof(discountAmount));

            // Discount cannot exceed SubTotal + ShippingFee
            var maxDiscount = SubTotal + ShippingFee;
            DiscountAmount = Math.Min(discountAmount, maxDiscount);

            CalculateTotals();
        }

        /// <summary>
        /// Add item to order and recalculate
        /// </summary>
        public void AddItem(OrderItem item)
        {
            Items.Add(item);
            CalculateTotals();
        }

        /// <summary>
        /// Remove item from order and recalculate
        /// </summary>
        public void RemoveItem(OrderItem item)
        {
            Items.Remove(item);
            CalculateTotals();
        }

        /// <summary>
        /// Update order status with validation
        /// </summary>
        public void UpdateStatus(OrderStatus newStatus)
        {
            // Business rules for status transitions
            if (Status == OrderStatus.Cancelled && newStatus != OrderStatus.Cancelled)
                throw new InvalidOperationException("Cannot change status of a cancelled order");

            if (Status == OrderStatus.Delivered && newStatus != OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot change status of a delivered order");

            Status = newStatus;

            if (newStatus == OrderStatus.Delivered)
                CompletedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Mark payment as completed
        /// </summary>
        public void MarkAsPaid()
        {
            if (PaymentStatus == PaymentStatus.Paid)
                throw new InvalidOperationException("Order is already paid");

            PaymentStatus = PaymentStatus.Paid;

            if (Status == OrderStatus.Pending)
                Status = OrderStatus.Processing;
        }

        /// <summary>
        /// Cancel order and restore stock
        /// </summary>
        public void Cancel()
        {
            if (Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Cannot cancel a delivered order");

            if (Status == OrderStatus.Cancelled)
                throw new InvalidOperationException("Order is already cancelled");

            Status = OrderStatus.Cancelled;

            // Stock restoration will be handled by domain event handlers
        }
    }
}
