using System;
using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
	public sealed class DiscountRepository : IDiscountRepository
	{
        private readonly NpgsqlConnection _connection;

        public DiscountRepository(IConfiguration configuration)
		{
            var connectionString = configuration.GetValue<string?>("DatabaseSettings:ConnectionString") ?? throw new ArgumentNullException(nameof(configuration));
            _connection = new NpgsqlConnection(connectionString);
		}

        public async Task<bool> CreateDiscountAsync(Coupon coupon, CancellationToken cancellationToken)
        {
            CommandDefinition commandDefinition = new("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                                            coupon, cancellationToken: cancellationToken);

            var affected = await _connection.ExecuteAsync(commandDefinition);

            return affected > 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName, CancellationToken cancellationToken)
        {
            CommandDefinition commandDefinition = new("DELETE FROM Coupon WHERE ProductName = @ProductName",
                                            new { ProductName = productName }, cancellationToken: cancellationToken);

            var affected = await _connection.ExecuteAsync(commandDefinition);

            return affected > 0;
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public Task<Coupon?> GetDiscountAsync(string productName, CancellationToken cancellationToken)
        {
            CommandDefinition commandDefinition = new("SELECT * FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName }, cancellationToken: cancellationToken);

            return _connection.QueryFirstOrDefaultAsync<Coupon?>(commandDefinition);
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon, CancellationToken cancellationToken)
        {
            CommandDefinition commandDefinition = new("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
                                            coupon, cancellationToken: cancellationToken);

            var affected = await _connection.ExecuteAsync(commandDefinition);

            return affected > 0;
        }
    }
}

