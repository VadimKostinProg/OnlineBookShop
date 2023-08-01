using Dimain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.DTO
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }
    }

    public static class CategoryExt
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                DisplayOrder = category.DisplayOrder,
            };
        }
    }
}
