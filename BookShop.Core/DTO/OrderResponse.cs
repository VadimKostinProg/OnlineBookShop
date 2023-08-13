using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.DTO
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
        public decimal TotalPrice { get; set; }
        public DateTime OrderDateTime { get; set; }
    }
}
