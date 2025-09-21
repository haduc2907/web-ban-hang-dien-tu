﻿namespace WEB.Models
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}
