using EShopAI.ApiService.Models;
using EShopAI.ApiService.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EShopAI.ApiService.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers").WithTags("Customers");

        group.MapGet("/", (CustomerService service) => TypedResults.Ok(service.GetAll()));

        group.MapGet("/{id:guid}", Results<Ok<Customer>, NotFound> (Guid id, CustomerService service) =>
            service.GetById(id) is { } customer ? TypedResults.Ok(customer) : TypedResults.NotFound());

        group.MapPost("/", (Customer customer, CustomerService service) =>
        {
            var created = service.Create(customer);
            return TypedResults.Created($"/api/customers/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", Results<NoContent, NotFound> (Guid id, Customer customer, CustomerService service) =>
            service.Update(id, customer) ? TypedResults.NoContent() : TypedResults.NotFound());

        group.MapDelete("/{id:guid}", Results<NoContent, NotFound> (Guid id, CustomerService service) =>
            service.Delete(id) ? TypedResults.NoContent() : TypedResults.NotFound());

        return app;
    }
}
