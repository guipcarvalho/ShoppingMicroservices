using Discount.Grpc.Repositories;
using Discount.Grpc.Services;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddGrpc();

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .ConfigureGlobalProcessorOptions(opt =>
        {
            opt.ProviderSwitches = "Force Quote=false";
        })
        .WithGlobalConnectionString(builder.Configuration.GetValue<string>("DatabaseSettings:ConnectionString"))
        .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());


var app = builder.Build();

using var scope = app.Services.CreateScope();
var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

