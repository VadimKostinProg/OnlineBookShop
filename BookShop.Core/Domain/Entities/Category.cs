using BookShop.Core.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dimain.Entities
{
    public class Category : EntityBase
    {
        [DisplayName("Category Name")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        [DisplayName("Display Order")]
        [Range(1,100)]
        public int DisplayOrder { get; set; }
    }
}
