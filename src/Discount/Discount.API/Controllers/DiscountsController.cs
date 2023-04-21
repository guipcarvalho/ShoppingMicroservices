using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/discounts")]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _repository;

        public DiscountsController(IDiscountRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get a product discount
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>Product example: IPhone X</remarks>
		[HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName, CancellationToken cancellationToken)
        {
            var coupon = await _repository.GetDiscountAsync(productName, cancellationToken);

            if (coupon is not null)
                return Ok(coupon);

            return NotFound();
        }

        /// <summary>
        /// Create a product discount
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>{  
	    ///      "productName": "Huawei Plus",
	    ///      "description": "test new product",
	    ///      "amount": 550
        ///        }
        ///</remarks>
        [HttpPost]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon, CancellationToken cancellationToken)
        {
            await _repository.CreateDiscountAsync(coupon, cancellationToken);

            return CreatedAtRoute(nameof(GetDiscount), new { productName = coupon.ProductName }, coupon);
        }

        /// <summary>
        /// Update a product discount
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="cancellationToken"></param>
        /// <remarks>{"id":3,"productName":"Huawei Plus","description":"test new product","amount":400}</remarks>
        [HttpPut]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon, CancellationToken cancellationToken)
        {
            var updateResult = await _repository.UpdateDiscountAsync(coupon, cancellationToken);

            if (updateResult)
                return Ok(coupon);

            return BadRequest();
        }

        /// <summary>
        /// Delete all discount of a product
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{productName}")]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> DeleteDiscount(string productName, CancellationToken cancellationToken)
        {
            return Ok(await _repository.DeleteDiscountAsync(productName, cancellationToken));
        }
    }
}

