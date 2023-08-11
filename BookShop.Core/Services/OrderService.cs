using BookShop.Core.Domain.Entities;
using BookShop.Core.Domain.RepositoryContracts;
using BookShop.Core.DTO;
using BookShop.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IShoppingCartService _shoppingCartService;

        public OrderService(IRepository repository, UserManager<ApplicationUser> userManager, IShoppingCartService shoppingCartService)
        {
            _repository = repository;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            if (!(await _repository.ExistsAsync<Order>(order => order.Id == id)))
                throw new KeyNotFoundException("Order with such Id is not found.");

            await _repository.DeleteAsync<Order>(id);
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync<Order>(id, includeStrings: "ApplicationUser");

            if(order is null)
                throw new KeyNotFoundException("Order with such Id is not found.");

            return await ConvertOrderToOrderResponseAsync(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersAsync(Expression<Func<OrderResponse, bool>>? predicate = null)
        {
            var orders = await _repository.GetAllAsync<Order>(predicate: null, includeStrings: "ApplicationUser");

            var result = new List<OrderResponse>();

            foreach (var order in orders)
            {
                var orderResponse = await ConvertOrderToOrderResponseAsync(order);
                result.Add(orderResponse);
            }

            return result;
        }

        private async Task<OrderResponse> ConvertOrderToOrderResponseAsync(Order order)
        {
            var orderItems = await _repository
                .GetAllAsync<OrderItem>(item => item.OrderId == order.Id, includeStrings: "Product");

            var items = orderItems
                .Select(item => new OrderItemResponse()
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Title,
                    Count = item.Count,
                    Price = item.Price,
                    IsDiscountActive = item.IsDiscountActive,
                    DiscountAmount = item.DiscountAmount,
                })
                .ToList();

            var orderResponse = new OrderResponse()
            {
                Id = order.Id,
                Items = items,
                TotalPrice = items.Sum(item => item.DiscountPrice),
                UserId = order.UserId,
                UserName = order.User.PersonName,
            };

            return orderResponse;
        }

        public async Task ProceedOrderAsync(OrderProceedRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId.ToString()) is null)
                throw new KeyNotFoundException("User with such Id is not found.");

            //Adding order entity
            var order = request.ToOrder();
            await _repository.AddAsync(order);

            //Adding order items
            var shoppingCart = await _shoppingCartService.GetShoppingCartByUserAsync(request.UserId);

            foreach (var item in shoppingCart.Items)
            {
                var orderItem = item.ToOrderItem();
                await _repository.AddAsync(orderItem);
            }

            //Clearing shopping cart
            await _shoppingCartService.ClearShoppingCartAsync(request.UserId);
        }
    }
}
