using Catalog.API.Config;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(DatabaseSettings.ConfigName)
                .Get<DatabaseSettings>() 
                ?? throw new ArgumentNullException(DatabaseSettings.ConfigName);
            
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            Products = database.GetCollection<Product>(databaseSettings.CollectionName);
            CatalogContextSeed.SeedData(Products).Wait();
        }

        public IMongoCollection<Product> Products { get; init; }
    }
}
