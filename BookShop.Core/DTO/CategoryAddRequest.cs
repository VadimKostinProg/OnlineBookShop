using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
    }
}
