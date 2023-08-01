using BookShop.Core.DTO;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for managing discount in the application.
    /// </summary>
    public interface IDiscountService
    {
        /// <summary>
        /// Method for creating new discount for product.
        /// </summary>
        /// <param name="request">Discount set request DTO model, which contains information
        /// about product, amount of product and amount of discount.</param>
        Task CreateDiscountAsync(DiscountSetRequest request);

        /// <summary>
        /// Method for reading the discount amount for product.
        /// </summary>
        /// <param name="productId">Guid of product to read the discount amount.</param>
        /// <param name="count">Count of product to read the discount amount.</param>
        /// <returns>Value of discount amount between 1 and 100, if discount for the 
        /// product does not exist, it returns 0.</returns>
        Task<double> GetDiscountByProductAsync(Guid productId, int count);

        /// <summary>
        /// Method for deleting the all discounts for the product.
        /// </summary>
        /// <param name="productId">Guid of the product to delete all discounts.</param>
        Task DeleteAllDiscountsAsync(Guid productId);

        /// <summary>
        /// Method for deleting the discount for the product only for some certian amount of product.
        /// </summary>
        /// <param name="productId">Guid of the product to delete discount.</param>
        /// <param name="count">Amount of product to delete discount.</param>
        Task DeleteDiscountAsync(Guid productId, int count);
    }
}
