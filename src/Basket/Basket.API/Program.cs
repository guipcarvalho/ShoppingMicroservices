using Basket.API.Repositories;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Basket.API.GrpcServices;
using Discount.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(opt =>
{
    var configPath = "CacheSettings:ConnectionString";
    opt.Configuration = builder.Configuration.GetValue<string>(configPath) ?? throw new ArgumentNullException(configPath);
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{
    opt.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl") ?? throw new ArgumentNullException("GrpcSettings:DiscountUrl"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Basket API",
        Description = "A Microservice to manage user baskets",
        Contact = new OpenApiContact
        {
            Name = "Guilherme Carvalho",
            Url = new Uri("https://github.com/guipcarvalho")
        }
    });

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

