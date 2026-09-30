using EShopAI.ApiService.Models;
using EShopAI.ApiService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopAI.ApiService.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapGet("/", (OrderService service) => TypedResults.Ok(service.GetAll()));

        group.MapGet("/{id:int}", Results<Ok<Order>, NotFound> (int id, OrderService service) =>
            service.GetById(id) is { } order ? TypedResults.Ok(order) : TypedResults.NotFound());

        group.MapPost("/", (Order order, OrderService service) =>
        {
            var created = service.Create(order);
            return TypedResults.Created($"/api/orders/{created.Id}", created);
        });

        group.MapPut("/{id:int}", Results<NoContent, NotFound> (int id, Order order, OrderService service) =>
            service.Update(id, order) ? TypedResults.NoContent() : TypedResults.NotFound());

        group.MapDelete("/{id:int}", Results<NoContent, NotFound> (int id, OrderService service) =>
            service.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound());

        return app;
    }
}
