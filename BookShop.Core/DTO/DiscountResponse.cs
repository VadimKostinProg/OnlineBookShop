using BookShop.Core.Domain.Entities;

namespace BookShop.Core.DTO
{
    public class DiscountResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public Dictionary<int , double> CountDiscountSet { get; set; } = new Dictionary<int , double>();
    }
}
