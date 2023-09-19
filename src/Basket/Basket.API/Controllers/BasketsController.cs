using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/baskets")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        
        public BasketsController(IBasketRepository repository, DiscountGrpcService discountGrpcService, IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        /// <summary>
        /// Gets the user basket
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>userName example: swn</remarks>
        /// <returns></returns>
        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var basket = await _repository.GetBasketAsync(userName, cancellationToken);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        /// <summary>
        /// Update or create a user basket
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>
        /// Payload example:
        /// {
        ///      "UserName": "swn",
        ///      "Items": [
        ///        {
        ///          "Quantity": 2,
        ///          "Color": "Red",
        ///          "Price": 500,
        ///          "ProductId": "60210c2a1556459e153f0554",
        ///          "ProductName": "IPhone X"
        ///        },
        ///        {
        ///          "Quantity": 1,
        ///          "Color": "Blue",
        ///          "Price": 500,
        ///          "ProductId": "60210c2a1556459e153f0555",
        ///          "ProductName": "Samsung 10"
        ///        }
        ///      ]  
        ///   }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket, CancellationToken cancellationToken)
        {
            foreach (var item in basket.Items)
            {
                try
                {
                    var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName, cancellationToken);
                    item.Price -= coupon.Amount;
                }
                catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound) { } //No work should be done if there is not a coupon for the product
            }

            return Ok(await _repository.UpdateBasketAsync(basket, cancellationToken));
        }


        /// <summary>
        /// Delete a user basket
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        ///<remarks>userName example: swn</remarks>
        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            await _repository.DeleteBasketAsync(userName, cancellationToken);
            return Accepted();
        }
        
        [HttpPost("[action]", Name = "Checkout")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout, CancellationToken cancellationToken)
        {
            if (basketCheckout?.UserName is null) return BadRequest();
            
            var basket = await _repository.GetBasketAsync(basketCheckout.UserName, cancellationToken);
            if (basket is null) return BadRequest();
            
            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMessage, cancellationToken);
            
            await _repository.DeleteBasketAsync(basketCheckout.UserName, cancellationToken);
            
            return Accepted();
        }
    }
}

