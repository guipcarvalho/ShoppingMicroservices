﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/baskets")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketsController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
    }
}
