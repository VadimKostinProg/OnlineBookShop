using BookShop.Core.Domain.Entities;

namespace BookShop.Core.DTO
{
    public class DiscountResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public double DiscountAmount { get; set; }
    }

    public static class DiscountExt
    {
        public static DiscountResponse ToDiscountResponse(this Discount discount)
        {
            return new DiscountResponse()
            {
                Id = discount.Id,
                ProductId = discount.ProductId,
                ProductName = discount.Product.Title,
                DiscountAmount = discount.DiscountAmount
            };
        }
    }
}
