using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BookShop.Core.Domain.Entities;

namespace BookShop.Core.DTO
{
    public class ProductAddRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string ISBN { get; set; } = null!;

        [Required]
        public string Author { get; set; } = null!;

        [Required]
        [DisplayName("List price")]
        [Range(1, 1000)]
        public decimal Price { get; set; }

        [Required]
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        public Product ToProduct()
        {
            return new Product()
            {
                Id = Guid.NewGuid(),
                Title = this.Title,
                Description = this.Description,
                ISBN = this.ISBN,
                Author = this.Author,
                Price = this.Price,
                CategoryId = this.CategoryId,
                ImageUrl = this.ImageUrl,
            };
        }
    }
}
