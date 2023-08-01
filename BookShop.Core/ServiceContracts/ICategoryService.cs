using BookShop.Core.DTO;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing categories.
    /// </summary>
    public interface ICategoryService : 
        ICRUDService<CategoryAddRequest, CategoryUpdateRequest, CategoryResponse>, 
        INameGetter<CategoryResponse>
    {
    }
}
