namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public required int Quantity { get; set; }
        public string? Color { get; set; }
        public required decimal Price { get; set; }
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
    }
}

