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
    }
}
