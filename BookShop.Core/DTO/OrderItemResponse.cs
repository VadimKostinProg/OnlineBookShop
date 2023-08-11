﻿namespace BookShop.Core.DTO
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
    }
}
