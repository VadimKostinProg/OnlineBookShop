using BookShop.Core.Domain.Entities;

namespace BookShop.Core.DTO
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int DisplayOrder { get; set; }

        public CategoryUpdateRequest ToCategoryUpdateRequest()
        {
            return new CategoryUpdateRequest()
            {
                Id = this.Id,
                Name = this.Name,
                DisplayOrder = this.DisplayOrder,
            };
        }
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
