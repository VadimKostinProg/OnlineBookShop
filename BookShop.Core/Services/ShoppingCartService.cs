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

        public async Task AddShoppingCartItemAsync(ShoppingCartItemAddRequest request)
        {
            //Validation
            if (request is null)
                throw new ArgumentNullException("Shopping cart is null or empty.");

            if (!(await _repository.ExistsAsync<Product>(product => product.Id == request.ProductId)))
                throw new ArgumentException("Product with such Id does not exist.");

            if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
                throw new ArgumentException("User with such Id does not exist.");

            if (request.Count < 1 || request.Count > 1000)
                throw new ArgumentException("Amount of product must be between 1 and 1000");

            var shoppingCart = request.ToShoppingCart();
            await _repository.AddAsync(shoppingCart);
        }

        public async Task DeleteShoppingCartAsync(Guid userId)
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
                throw new ArgumentException("User with such Id does not exist.");

            var orderItems = await this.GetOrderItemsAsync(userId);

            var shoppingCartResponse = new ShoppingCartResponse()
            {
                UserId = userId,
                Items = orderItems,
                TotalPrice = orderItems.Sum(item => item.Price)
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
                var discount = await _discountService.GetDiscountByProductAsync(shoppingCart.ProductId, shoppingCart.Count);
                bool isDiscountActive = discount == 0;

                var price = isDiscountActive ?
                    (decimal)((double)shoppingCart.Product.Price * (1 - discount)) :
                    shoppingCart.Product.Price;

                var orderItem = new OrderItemResponse()
                {
                    ProductId = shoppingCart.ProductId,
                    ProductName = shoppingCart.Product.Title,
                    ImageUrl = shoppingCart.Product.ImageUrl,
                    Count = shoppingCart.Count,
                    IsDiscountActive = isDiscountActive,
                    Price = price
                };

                orderItems.Add(orderItem);
            }

            return orderItems;
        }
    }
}
