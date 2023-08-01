namespace BookShop.Core.DTO
{
    public class OrderItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Count { get; set; }
        public bool IsDiscountActive { get; set; }
        public decimal Price { get; set; }
    }
}
