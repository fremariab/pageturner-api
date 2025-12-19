using System;
using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using PageTurner.Api.Data;
using PageTurner.Api.Infrastructure.Elasticsearch;
using PageTurner.Api.Services.Implementations;
using PageTurner.Api.Services.Interfaces;

// Add this namespace if your Redis files are here:
// using PageTurner.Api.Infrastructure.Redis;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure Kestrel (Server)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000); // HTTP
    options.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps()); // HTTPS
});
builder.Services.AddDbContext<PageTurnerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2. Add Services (Dependency Injection)
// 👇 CRITICAL: This line enables your Controllers!
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Configure Infrastructure (Redis & Elasticsearch)
var redisConnectionString = builder.Configuration["Redis:ConnectionString"];

// ensure you have the correct using statement for your RedisConnection class
builder.Services.AddSingleton(
    new PageTurner.Api.Infrastructure.Redis.RedisConnection(redisConnectionString!)
);

builder.Services.AddSingleton<
    PageTurner.Api.Services.Interfaces.ICacheService,
    PageTurner.Api.Services.Implementations.RedisCacheService
>();

builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    var provider = new ElasticClientProvider(builder.Configuration);
    return provider.Client;
});

// 4. Register your Application Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

// 5. Configure the Request Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
