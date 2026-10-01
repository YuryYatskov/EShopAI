using EShopAI.Web.Models;

namespace EShopAI.Web.Services;

public class ApiClient(HttpClient http)
{
    public async Task<List<Product>> GetProductsAsync() =>
        await http.GetFromJsonAsync<List<Product>>("/api/products") ?? [];

    public async Task<ShoppingCart> GetCartAsync(string email) =>
        await http.GetFromJsonAsync<ShoppingCart>($"/api/carts/{Uri.EscapeDataString(email)}") ?? new() { CustomerEmail = email };

    public async Task<ShoppingCart> AddToCartAsync(string email, int productId, int quantity = 1)
    {
        var response = await http.PostAsJsonAsync($"/api/carts/{Uri.EscapeDataString(email)}/items", new AddCartItemRequest(productId, quantity));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ShoppingCart>())!;
    }

    public async Task RemoveFromCartAsync(string email, int productId)
    {
        var response = await http.DeleteAsync($"/api/carts/{Uri.EscapeDataString(email)}/items/{productId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Order> CheckoutAsync(string email, string customerName)
    {
        var response = await http.PostAsJsonAsync($"/api/carts/{Uri.EscapeDataString(email)}/checkout", new CheckoutRequest(customerName));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Order>())!;
    }
}
