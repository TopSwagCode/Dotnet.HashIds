using Dotnet.HashIds.API;
using Dotnet.HashIds.Database;
using HashidsNet;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<ProductId>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("productId")
    });
});
builder.Services.AddSingleton(new ProductDatebase());
builder.Services.AddSingleton<IHashids>(_ => new Hashids("Our super secret salt!", 5));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();