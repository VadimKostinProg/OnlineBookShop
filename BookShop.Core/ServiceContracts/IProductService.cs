using BookShop.Core.DTO;
using System.Linq.Expressions;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing products.
    /// </summary>
    public interface IProductService : 
        ICRUDService<ProductAddRequest, ProductUpdateRequest, ProductResponse>, 
        INameGetter<ProductResponse>
    {
        Task<IEnumerable<ProductResponse>> GetFilteredAsync(Expression<Func<ProductResponse, bool>> predicate);
    }
}
