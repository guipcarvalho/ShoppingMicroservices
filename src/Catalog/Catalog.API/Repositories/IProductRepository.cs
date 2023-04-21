using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken);
        Task<Product?> GetProductByIdAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetProductByNameAsync(string name, CancellationToken cancellationToken);
        Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName, CancellationToken cancellationToken);
        Task CreateProductAsync(Product product, CancellationToken cancellationToken);
        Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken);
        Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken);
    }
}
