using BookShop.Core.DTO;

namespace BookShop.Core.ServiceContracts
{
    public interface IShoppingCartService
    {
        Task AddShoppingCartItemAsync(ShoppingCartItemAddRequest request);
        Task<ShoppingCartResponse> GetShoppingCartByUserAsync(Guid userId);
        Task DeleteShoppingCartAsync(Guid userId);
    }
}
