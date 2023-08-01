using BookShop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.ServiceContracts
{
    public interface IOrderService
    {
        Task ProceedOrderAsync(OrderProceedRequest request);
        Task<OrderResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetOrdersAsync(Expression<Func<OrderResponse, bool>> predicate);
        Task DeleteOrderAsync(Guid id);
    }
}
