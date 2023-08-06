using BookShop.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Core.Domain.Entities
{
    public class ShoppingCart : EntityBase
    {
        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Count { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
