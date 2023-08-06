using BookShop.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.DTO
{
    public class DiscountSetRequest
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(2, 1000)]
        public int Count { get; set; }

        [Required]
        [Range(1, 100)]
        public double DiscountAmount { get; set; }

        public Discount ToDiscount()
        {
            return new Discount()
            {
                Id = Guid.NewGuid(),
                ProductId = this.ProductId,
                Count = this.Count,
                DiscountAmount = this.DiscountAmount
            };
        }
    }
}
