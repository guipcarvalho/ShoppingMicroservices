using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _protoServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient protoServiceClient)
        {
            _protoServiceClient = protoServiceClient ?? throw new ArgumentNullException(nameof(protoServiceClient));
        }

        public async Task<CouponModel> GetDiscountAsync(string productName, CancellationToken cancellationToken)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _protoServiceClient.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);
        }
    }
}
