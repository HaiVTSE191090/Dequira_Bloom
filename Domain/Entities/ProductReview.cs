using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public class ProductReview : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public bool IsVerifiedPurchase { get; set; } = false;
        public bool IsApproved { get; set; } = false;

        // Navigation Properties
        public virtual Product Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual Order? Order { get; set; }
    }
}
