using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken);
        Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken);
        Task DeleteBasketAsync(string userName, CancellationToken cancellationToken);
    }
}

