using MongoDB.Driver;

namespace Catalog.API.Config
{
    public record DatabaseSettings
    {
        public const string ConfigName = "DatabaseSettings";

        public required string ConnectionString { get; init; }
        public required string DatabaseName { get; init; }
        public required string CollectionName { get; init; }
    }
}
