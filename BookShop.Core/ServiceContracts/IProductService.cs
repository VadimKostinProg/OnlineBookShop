using BookShop.Core.DTO;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing products.
    /// </summary>
    public interface IProductService : 
        ICRUDService<ProductAddRequest, ProductUpdateRequest, ProductResponse>, 
        INameGetter<ProductResponse>
    {
    }
}
