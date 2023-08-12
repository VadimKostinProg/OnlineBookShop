using BookShop.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.DTO
{
    public class OrderProceedRequest
    {
        public Guid UserId { get; set; }
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public Order ToOrder()
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                UserId = this.UserId,
                City = this.City,
                Address = this.Address,
                PostalCode = this.PostalCode,
                PhoneNumber = this.PhoneNumber,
                OrderDateTime = DateTime.Now
            };
        }
    }
}
