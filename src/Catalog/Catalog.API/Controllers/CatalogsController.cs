using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/catalogs")]
    [Produces("application/json")]
    public class CatalogsController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogsController> _logger;

        public CatalogsController(
            IProductRepository repository,
            ILogger<CatalogsController> logger
        )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetProductsAsync(cancellationToken));
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(id, cancellationToken);

            if (product is not null) return Ok(product);

            _logger.LogWarning("Product with id: {id} not found", id);
            return NotFound();
        }

        [HttpGet("categories/{categoryName}", Name = "GetProductByCategory")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductByCategory(string categoryName, CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetProductByCategoryAsync(categoryName, cancellationToken));
        }
    }
}
