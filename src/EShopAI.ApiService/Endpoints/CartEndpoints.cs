using EShopAI.ApiService.Models;
using EShopAI.ApiService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopAI.ApiService.Endpoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/carts").WithTags("Carts");

        group.MapGet("/{email}", (string email, CartService service) => TypedResults.Ok(service.GetByEmail(email)));

        group.MapPost("/{email}/items", Results<Ok<ShoppingCart>, NotFound, BadRequest<string>> (string email, AddCartItemRequest request, CartService service) =>
        {
            var (status, cart) = service.AddItem(email, request.ProductId, request.Quantity);
            return status switch
            {
                CartStatus.Ok => TypedResults.Ok(cart!),
                CartStatus.ProductNotFound => TypedResults.NotFound(),
                _ => TypedResults.BadRequest("Quantity must be greater than zero.")
            };
        });

        group.MapDelete("/{email}/items/{productId:int}", Results<NoContent, NotFound> (string email, int productId, CartService service) =>
            service.RemoveItem(email, productId) ? TypedResults.NoContent() : TypedResults.NotFound());

        group.MapDelete("/{email}", (string email, CartService service) =>
        {
            service.Clear(email);
            return TypedResults.NoContent();
        });

        group.MapPost("/{email}/checkout", Results<Created<Order>, BadRequest<string>> (string email, CheckoutRequest request, CartService service) =>
        {
            var (status, order) = service.Checkout(email, request.CustomerName);
            return status == CartStatus.Ok
                ? TypedResults.Created($"/api/orders/{order!.Id}", order)
                : TypedResults.BadRequest("Cart is empty.");
        });

        return app;
    }
}
