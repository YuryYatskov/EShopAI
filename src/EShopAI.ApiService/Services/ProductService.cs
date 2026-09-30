using EShopAI.ApiService.Models;

namespace EShopAI.ApiService.Services;

public class ProductService
{
    private readonly List<Product> _products =
    [
        new() { Id = 1, Name = "Wireless Mouse", Description = "Ergonomic 2.4GHz wireless mouse", Price = 24.99m, ImageUrl = "https://example.com/images/mouse.jpg" },
        new() { Id = 2, Name = "Mechanical Keyboard", Description = "RGB mechanical keyboard with blue switches", Price = 79.90m, ImageUrl = "https://example.com/images/keyboard.jpg" },
        new() { Id = 3, Name = "27\" 4K Monitor", Description = "IPS 4K UHD monitor with HDR support", Price = 329.00m, ImageUrl = "https://example.com/images/monitor.jpg" },
        new() { Id = 4, Name = "USB-C Hub", Description = "7-in-1 USB-C hub with HDMI and card reader", Price = 39.50m, ImageUrl = "https://example.com/images/hub.jpg" },
        new() { Id = 5, Name = "Noise-Cancelling Headphones", Description = "Bluetooth over-ear headphones with ANC", Price = 149.00m, ImageUrl = "https://example.com/images/headphones.jpg" }
    ];

    private readonly Lock _lock = new();
    private int _nextId = 6;

    public IReadOnlyList<Product> GetAll()
    {
        lock (_lock)
        {
            return [.. _products];
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            return _products.FirstOrDefault(product => product.Id == id);
        }
    }

    public Product Create(Product product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }
    }

    public bool Update(int id, Product updatedProduct)
    {
        lock (_lock)
        {
            var existingProduct = _products.FirstOrDefault(product => product.Id == id);
            if (existingProduct is null)
            {
                return false;
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.ImageUrl = updatedProduct.ImageUrl;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _products.RemoveAll(product => product.Id == id) > 0;
        }
    }
}
