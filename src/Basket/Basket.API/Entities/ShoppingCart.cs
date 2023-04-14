using System;
namespace Basket.API.Entities
{
    public record ShoppingCart
	{
		public string? UserName { get; set; }

		public IList<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

		public decimal TotalPrice => ShoppingCartItems.Sum(x => x.Quantity * x.Price);

		public ShoppingCart() { }

		public ShoppingCart(string userName)
		{
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
	}
}

