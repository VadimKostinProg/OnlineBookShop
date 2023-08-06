using BookShop.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Core.DTO
{
    public class CategoryUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [Range(1, 100)]
        public int DisplayOrder { get; set; }

        public Category ToCategory()
        {
            return new Category()
            {
                Id = this.Id,
                Name = this.Name,
                DisplayOrder = this.DisplayOrder
            };
        }
    }
}
