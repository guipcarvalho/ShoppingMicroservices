using System;
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

		[HttpGet("{productName}", Name = "GetDiscount")]
		[ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName, CancellationToken cancellationToken)
		{
			var coupon = await _repository.GetDiscountAsync(productName, cancellationToken);

			if(coupon is not null)
				return Ok(coupon);

			return NotFound();
		}

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status201Created)]
        public async Task<ActionResult<Coupon>> CreateDiscount(Coupon coupon, CancellationToken cancellationToken)
        {
            await _repository.CreateDiscountAsync(coupon, cancellationToken);

            return CreatedAtAction(nameof(GetDiscount), new { productName = coupon.ProductName }, coupon);
        }
    }
}

