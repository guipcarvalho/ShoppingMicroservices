using System;
using Discount.API.Entities;

namespace Discount.API.Repositories
{
	public interface IDiscountRepository
	{
		Task<Coupon?> GetDiscountAsync(string productName, CancellationToken cancellationToken);
		Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);
		Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);
        Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken);
	}
}

