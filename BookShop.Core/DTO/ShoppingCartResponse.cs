namespace BookShop.Core.DTO
{
    public class ShoppingCartResponse
    {
        public Guid UserId { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
        public decimal TotalPrice { get; set; }
    }
}
