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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetProductsAsync(cancellationToken));
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id, CancellationToken cancellationToken)
        {
            var product = await _repository.GetProductByIdAsync(id, cancellationToken);

            if (product is not null) return Ok(product);

            _logger.LogWarning("Product with id: {id} not found", id);
            return NotFound();
        }

        /// <summary>
        /// Get products by category name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("categories/{categoryName}", Name = "GetProductByCategory")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName, CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetProductByCategoryAsync(categoryName, cancellationToken));
        }

        /// <summary>
        /// Creates a product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product, CancellationToken cancellationToken)
        {
            await _repository.CreateProductAsync(product, cancellationToken);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        /// <summary>
        /// Updates profuct with the corresponding id
        /// </summary>
        /// <param name="product"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product, CancellationToken cancellationToken)
        {
            var changed = await _repository.UpdateProductAsync(product, cancellationToken);

            if (changed)
                return Ok(product);

            return NotFound();
        }

        /// <summary>
        /// Delete product with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(string id, CancellationToken cancellationToken)
        {
            var changed = await _repository.DeleteProductAsync(id, cancellationToken);

            if (changed)
                return NoContent();

            return NotFound();
        }
    }
}
