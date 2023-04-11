using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken)
    {
        return await _context
            .Products
            .Find(Builders<Product>.Filter.Empty)
            .ToListAsync(cancellationToken);
    }

    public Task<Product> GetProductByIdAsync(string id, CancellationToken cancellationToken)
    {
        return _context
            .Products
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductByNameAsync(string name, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Name, name);

        return await _context
            .Products
            .Find(filter)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Category, categoryName);

        return await _context
            .Products
            .Find(filter)
            .ToListAsync(cancellationToken);
    }

    public Task CreateProductAsync(Product product, CancellationToken cancellationToken)
    {
        return _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        var updateResult = await _context
            .Products
            .ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product, cancellationToken: cancellationToken);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Product>.Filter.Eq(x => x.Id, id);

        var deleteResult = await _context
            .Products
            .DeleteOneAsync(filter, cancellationToken);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}