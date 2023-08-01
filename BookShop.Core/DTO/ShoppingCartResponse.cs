namespace BookShop.Core.DTO
{
    public class ShoppingCartResponse
    {
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalPrice { get; set; }
    }
}
