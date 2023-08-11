using BookShop.Core.Domain.Entities;

namespace BookShop.Core.DTO
{
    public class OrderItemResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public bool IsDiscountActive { get; set; }
        public double DiscountAmount { get; set; }
        public decimal DiscountPrice
        {
            get
            {
                return (decimal)((double)Price * (1 - DiscountAmount / 100));
            }
        }

        public OrderItem ToOrderItem()
        {
            return new OrderItem()
            {
                Id = Guid.NewGuid(),
                ProductId = this.ProductId,
                Count = this.Count,
                IsDiscountActive = this.IsDiscountActive,
                Price = this.Price,
                DiscountAmount = this.DiscountAmount,
                DiscountPrice = this.DiscountPrice
            };
        }
    }
}