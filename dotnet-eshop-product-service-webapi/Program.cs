using System.Reflection;
using eshop.product.service.application.Events;
using eshop.product.service.application.Products;
using eshop.product.service.domain.Events;
using eshop.product.service.domain.Products;
using eshop.product.service.persistence.Products;
using eshop.product.service.persistence.Uow;
using MassTransit;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configurations
IConfigurationSection databaseConfigurationSection = builder.Configuration.GetSection("DatabaseSettings");
IConfigurationSection rabbitmqConfigurationSection = builder.Configuration.GetSection("RabbitMqSettings");

// Persistence dependencies
BsonClassMap.RegisterClassMap<Product>(cm =>
{
    cm.AutoMap(); // Map fields automatically
    cm.SetIdMember(null); // Do not map id automatically
    cm.SetIgnoreExtraElements(true);
});

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(databaseConfigurationSection["MongoUri"]));

builder.Services.AddScoped<IProductRepository, ProductMongoDbRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application dependencies
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddMassTransit(c =>
{
    // Add consumers
    c.AddConsumer<ProductAddedToCartEventHandler>();
    c.AddConsumer<ProductRemovedFromCartEventHandler>();

    c.UsingRabbitMq((context, cfg) =>
    {
        // Configure rabbitmq connection
        cfg.Host(rabbitmqConfigurationSection["RabbitMqUri"], "/", h =>
        {
            h.Username(rabbitmqConfigurationSection["RabbitMqUser"]!);
            h.Password(rabbitmqConfigurationSection["RabbitMqPass"]!);
        });

        // Product added to cart consumer configuration
        cfg.ReceiveEndpoint("product.added.to.cart", e =>
        {
            e.Bind("product.added.to.cart", x =>
            {
                x.Durable = true;
                x.AutoDelete = false;
                x.ExchangeType = "fanout";
            });
            e.ConfigureConsumer<ProductAddedToCartEventHandler>(context);
        });

        // Product removed from cart consumer configuration
        cfg.ReceiveEndpoint("product.removed.from.cart", e =>
        {
            e.Bind("product.removed.from.cart", x =>
            {
                x.Durable = true;
                x.AutoDelete = false;
                x.ExchangeType = "fanout";
            });
            e.ConfigureConsumer<ProductRemovedFromCartEventHandler>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Hosting dependencies
builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "e-shop Products Service",
        Description = "Service used for managing products in the e-shop application",
    });

    // Configure XML comments
    string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapControllers();
app.Run();
