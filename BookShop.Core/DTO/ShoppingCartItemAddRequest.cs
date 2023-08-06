using BookShop.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.DTO
{
    public class ShoppingCartItemAddRequest
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Amount of products in your basket must be between 1 and 1000")]
        public int Count { get; set; }

        public ShoppingCart ToShoppingCart()
        {
            return new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = this.UserId,
                ProductId = this.ProductId,
                Count = this.Count
            };
        }
    }
}
