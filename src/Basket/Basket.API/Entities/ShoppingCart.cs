namespace Basket.API.Entities;

public record ShoppingCart
{
	public string? UserName { get; set; }

	public IList<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

	public decimal TotalPrice => Items.Sum(x => x.Quantity * x.Price);

	public ShoppingCart() { }

    public ShoppingCart(string userName)
	{
		UserName = userName ?? throw new ArgumentNullException(nameof(userName));
	}
}