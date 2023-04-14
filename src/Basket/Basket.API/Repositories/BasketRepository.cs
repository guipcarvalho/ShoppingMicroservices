using System.Text.Json;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories
{
	public class BasketRepository : IBasketRepository
	{
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
		{
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task DeleteBasketAsync(string userName, CancellationToken cancellationToken)
        {
            return _cache.RemoveAsync(userName, cancellationToken);
        }

        public async Task<ShoppingCart?> GetBasketAsync(string userName, CancellationToken cancellationToken)
        {
            var result = await _cache.GetStringAsync(userName, cancellationToken);

            return string.IsNullOrWhiteSpace(result) ? null : JsonSerializer.Deserialize<ShoppingCart>(result);
        }

        public async Task<ShoppingCart?> UpdateBasketAsync(ShoppingCart basket, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(basket.UserName)) throw new ArgumentNullException(nameof(basket.UserName));

            await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

            return await GetBasketAsync(basket.UserName, cancellationToken);
        }
    }
}

