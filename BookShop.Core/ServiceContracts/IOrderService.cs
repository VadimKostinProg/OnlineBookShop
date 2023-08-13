using BookShop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.ServiceContracts
{
    /// <summary>
    /// Service for manageing orders.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Method for proceeding the order.
        /// </summary>
        /// <param name="request">Order proceed request DTO model with order information.</param>
        Task ProceedOrderAsync(OrderProceedRequest request);

        /// <summary>
        /// Method for reading order by it`s id from the data base.
        /// </summary>
        /// <param name="id">Guid of order to read.</param>
        /// <returns>Order response DTO model.</returns>
        Task<OrderResponse> GetByIdAsync(Guid id);

        /// <summary>
        /// Method for reading all orders from the data base.
        /// </summary>
        /// <param name="predicate">Predicate to filter orders.</param>
        /// <returns>Collection IEnumerable of Order response DTO models.</returns>
        Task<IEnumerable<OrderResponse>> GetOrdersAsync(Expression<Func<OrderResponse, bool>>? predicate = null);

        /// <summary>
        /// Method for deleting order by it`s id from the data base.
        /// </summary>
        /// <param name="id">Guid of order to delete.</param>
        Task DeleteOrderAsync(Guid id);
    }
}
