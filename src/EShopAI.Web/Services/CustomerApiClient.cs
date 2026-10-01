using EShopAI.Web.Models;

namespace EShopAI.Web.Services;

public class CustomerApiClient(HttpClient httpClient)
{
    private const string Route = "/api/customers";

    public async Task<Customer[]> GetAllAsync() =>
        await httpClient.GetFromJsonAsync<Customer[]>(Route) ?? [];

    public async Task<Customer?> GetByIdAsync(Guid id) =>
        await httpClient.GetFromJsonAsync<Customer>($"{Route}/{id}");

    public async Task<Customer> CreateAsync(Customer customer)
    {
        var response = await httpClient.PostAsJsonAsync(Route, customer);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Customer>())!;
    }

    public async Task UpdateAsync(Guid id, Customer customer)
    {
        var response = await httpClient.PutAsJsonAsync($"{Route}/{id}", customer);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"{Route}/{id}");
        response.EnsureSuccessStatusCode();
    }
}
