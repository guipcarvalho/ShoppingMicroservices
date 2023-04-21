using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository : IAsyncDisposable
    {
        Task<Coupon?> GetDiscountAsync(string productName, CancellationToken cancellationToken);
        Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);
        Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken);
        Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken);
    }
}

