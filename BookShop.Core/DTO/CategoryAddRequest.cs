using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BookShop.Core.Domain.Entities;

namespace BookShop.Core.DTO
{
    public class CategoryAddRequest
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [DisplayName("Display order")]
        [Range(1, 100)]
        public int DisplayOrder { get; set; }

        public Category ToCategory()
        {
            return new Category()
            {
                Id = Guid.NewGuid(),
                Name = this.Name,
                DisplayOrder = this.DisplayOrder,
            };
        }
    }
}
