using BookShop.Core.Domain.Entities;
using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository _repository;
        private readonly IDiscountService _discountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartService(IRepository repository, IDiscountService discountService,
            UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _discountService = discountService;
            _userManager = userManager;
        }

        public async Task SetShoppingCartItemAsync(ShoppingCartItemSetRequest request)
        {
            //Validation
            if (request is null)
                throw new ArgumentNullException("Shopping cart is null or empty.");

            if (!(await _repository.ExistsAsync<Product>(product => product.Id == request.ProductId)))
                throw new KeyNotFoundException("Product with such Id does not exist.");

            if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
                throw new KeyNotFoundException("User with such Id does not exist.");

            if (request.Count < 1 || request.Count > 1000)
                throw new ArgumentException("Amount of product must be between 1 and 1000");

            var shoppingCart = await _repository.FirstOrDefaultAsync<ShoppingCart>(item =>
            item.UserId == request.UserId && item.ProductId == request.ProductId);

            var shoppingCartToSet = request.ToShoppingCart();

            if (shoppingCart is null)
            {
                await _repository.AddAsync(shoppingCartToSet);
                return;
            }
            
            shoppingCartToSet.Id = shoppingCart.Id;
            await _repository.UpdateAsync(shoppingCartToSet);
        }

        public async Task ClearShoppingCartAsync(Guid userId)
        {
            var shoppingCarts = await _repository.GetAllAsync<ShoppingCart>(shoppingCart => shoppingCart.UserId == userId);

            foreach (var shoppingCart in shoppingCarts)
            {
                await _repository.DeleteAsync<ShoppingCart>(shoppingCart.Id);
            }
        }

        public async Task<ShoppingCartResponse> GetShoppingCartByUserAsync(Guid userId)
        {
            if (await _userManager.FindByIdAsync(userId.ToString()) is null)
                throw new KeyNotFoundException("User with such Id does not exist.");

            var orderItems = await this.GetOrderItemsAsync(userId);

            var shoppingCartResponse = new ShoppingCartResponse()
            {
                UserId = userId,
                Items = orderItems,
                TotalPrice = orderItems.Sum(item => item.DiscountPrice * item.Count)
            };

            return shoppingCartResponse;
        }

        private async Task<List<OrderItemResponse>> GetOrderItemsAsync(Guid userId)
        {
            var shoppingCarts = 
                await _repository.GetAllAsync<ShoppingCart>(shoppingCart => shoppingCart.UserId == userId,
                                                            includeStrings: "Product");

            var orderItems = new List<OrderItemResponse>();

            // Converting ShoppingCart entity to OrderItemResponse
            foreach (var shoppingCart in shoppingCarts)
            {
                var discount = await _discountService.GetDiscountAmountByProductAsync(shoppingCart.ProductId, shoppingCart.Count);

                var orderItem = new OrderItemResponse()
                {
                    ProductId = shoppingCart.ProductId,
                    ProductName = shoppingCart.Product.Title,
                    ImageUrl = shoppingCart.Product.ImageUrl,
                    Count = shoppingCart.Count,
                    IsDiscountActive = discount > 0,
                    Price = shoppingCart.Product.Price,
                    DiscountAmount = discount
                };

                orderItems.Add(orderItem);
            }

            return orderItems;
        }

        public async Task DeleteShoppingCartItemAsync(Guid userId, Guid productId)
        {
            if (!(await _repository.ExistsAsync<Product>(product => product.Id == productId)))
                throw new KeyNotFoundException("Product with such Id does not exist.");

            if (await _userManager.FindByIdAsync(userId.ToString()) is null)
                throw new KeyNotFoundException("User with such Id does not exist.");

            var shoppingCartItem = await _repository.FirstOrDefaultAsync<ShoppingCart>(item => 
            item.UserId == userId && item.ProductId == productId);

            if (shoppingCartItem is null)
                throw new KeyNotFoundException("Shopping cart item such user and product is not found.");

            await _repository.DeleteAsync<ShoppingCart>(shoppingCartItem.Id);
        }
    }
}
