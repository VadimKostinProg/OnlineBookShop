using BookShop.Core.DTO;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for shopping cart.
    /// </summary>
    public interface IShoppingCartService
    {
        /// <summary>
        /// Method for setting item in the shopping cart.
        /// </summary>
        /// <param name="request">Shopping cart set request DTO model with information
        /// of the customer and item to set.</param>
        Task SetShoppingCartItemAsync(ShoppingCartItemAddRequest request);

        /// <summary>
        /// Method for reading shopping cart items for the certain user.
        /// </summary>
        /// <param name="userId">Guid of user to read the shopping cart.</param>
        /// <returns>Shopping cart response DTO model.</returns>
        Task<ShoppingCartResponse> GetShoppingCartByUserAsync(Guid userId);

        /// <summary>
        /// Method for deleting certain item from the shopping cart.
        /// </summary>
        /// <param name="userId">Guid of user to delete item from the shopping cart.</param>
        /// <param name="productId">Guid of product to delete item from the shopping cart.</param>
        Task DeleteShoppingCartItemAsync(Guid userId, Guid productId);

        /// <summary>
        /// Method for clearing all shopping cart items for the certain user.
        /// </summary>
        /// <param name="userId">Guid of user to clear shopping cart.</param>
        Task ClearShoppingCartAsync(Guid userId);
    }
}
