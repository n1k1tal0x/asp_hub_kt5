using asp_hub_kt5.Interfaces;
using asp_hub_kt5.Models;
using asp_hub_kt5.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace asp_hub_kt5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString =
                "Host=localhost;Port=5432;Database=testbd;Username=postgres;Password=qwerty123;";

            // Add services to the container.
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.MapGet(
                "/api/products",
                async (IUnitOfWork unitOfWork) =>
                {
                    var products = await unitOfWork.Products.GetAllAsync();
                    return Results.Ok(products);
                }
            );

            app.MapGet(
                "/api/products/{id:int}",
                async Task<Results<Ok<Product>, NotFound>> (int id, IUnitOfWork unitOfWork) =>
                {
                    var product = await unitOfWork.Products.GetByIdAsync(id);
                    return product is null ? TypedResults.NotFound() : TypedResults.Ok(product);
                }
            );

            app.MapPost(
                "/api/products",
                async (Product product, IUnitOfWork unitOfWork) =>
                {
                    await unitOfWork.Products.AddAsync(product);
                    await unitOfWork.SaveChangesAsync();

                    return Results.Created($"/api/products/{product.Id}", product);
                }
            );

            app.MapPut(
                "/api/products/{id:int}",
                async Task<Results<NoContent, BadRequest<string>, NotFound>> (
                    int id,
                    Product product,
                    IUnitOfWork unitOfWork
                ) =>
                {
                    if (id != product.Id)
                    {
                        return TypedResults.BadRequest("Id in route does not match entity Id.");
                    }

                    var existingProduct = await unitOfWork.Products.GetByIdAsync(id);
                    if (existingProduct is null)
                    {
                        return TypedResults.NotFound();
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;

                    unitOfWork.Products.Update(existingProduct);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.NoContent();
                }
            );

            app.MapDelete(
                "/api/products/{id:int}",
                async Task<Results<NoContent, NotFound>> (int id, IUnitOfWork unitOfWork) =>
                {
                    var product = await unitOfWork.Products.GetByIdAsync(id);
                    if (product is null)
                    {
                        return TypedResults.NotFound();
                    }

                    unitOfWork.Products.Delete(product);
                    await unitOfWork.SaveChangesAsync();

                    return TypedResults.NoContent();
                }
            );

            app.Run();
        }
    }
}
