using EShopAI.ApiService.Models;
using EShopAI.ApiService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopAI.ApiService.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        group.MapGet("/", (ProductService service) => TypedResults.Ok(service.GetAll()));

        group.MapGet("/{id:int}", Results<Ok<Product>, NotFound> (int id, ProductService service) =>
            service.GetById(id) is { } product ? TypedResults.Ok(product) : TypedResults.NotFound());

        group.MapPost("/", (Product product, ProductService service) =>
        {
            var created = service.Create(product);
            return TypedResults.Created($"/api/products/{created.Id}", created);
        });

        group.MapPut("/{id:int}", Results<NoContent, NotFound> (int id, Product product, ProductService service) =>
            service.Update(id, product) ? TypedResults.NoContent() : TypedResults.NotFound());

        group.MapDelete("/{id:int}", Results<NoContent, NotFound> (int id, ProductService service) =>
            service.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound());

        return app;
    }
}
