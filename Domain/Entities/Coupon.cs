using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{


    public class Coupon : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();

        // Domain Methods
        /// <summary>
        /// Calculate discount amount for given order subtotal
        /// D_coupon calculation based on discount type
        /// </summary>
        public decimal CalculateDiscount(decimal orderSubTotal)
        {
            if (!IsValid())
                throw new InvalidOperationException("Coupon is not valid");

            if (MinimumOrderAmount.HasValue && orderSubTotal < MinimumOrderAmount.Value)
                throw new InvalidOperationException($"Order amount must be at least {MinimumOrderAmount.Value:C}");

            decimal discount = DiscountType switch
            {
                DiscountType.Percentage => orderSubTotal * (DiscountValue / 100m),
                DiscountType.FixedAmount => DiscountValue,
                _ => throw new InvalidOperationException("Invalid discount type")
            };

            // Apply max discount cap if specified
            if (MaxDiscountAmount.HasValue && discount > MaxDiscountAmount.Value)
                discount = MaxDiscountAmount.Value;

            // Discount cannot exceed order subtotal
            return Math.Min(discount, orderSubTotal);
        }

        /// <summary>
        /// Check if coupon is currently valid
        /// </summary>
        public bool IsValid()
        {
            var now = DateTime.UtcNow;
            return IsActive
                   && !IsDeleted
                   && now >= StartDate
                   && now <= EndDate
                   && (!UsageLimit.HasValue || UsedCount < UsageLimit.Value);
        }

        /// <summary>
        /// Increment usage count
        /// </summary>
        public void IncrementUsage()
        {
            if (!IsValid())
                throw new InvalidOperationException("Cannot use invalid coupon");

            UsedCount++;
        }

        /// <summary>
        /// Decrement usage count (e.g., when order is cancelled)
        /// </summary>
        public void DecrementUsage()
        {
            if (UsedCount > 0)
                UsedCount--;
        }
    }

}
