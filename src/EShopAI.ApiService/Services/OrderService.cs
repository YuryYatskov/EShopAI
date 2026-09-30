using EShopAI.ApiService.Models;

namespace EShopAI.ApiService.Services;

public class OrderService
{
    private readonly List<Order> _orders =
    [
        new()
        {
            Id = 1,
            CustomerName = "Ivan Petrenko",
            CustomerEmail = "ivan.petrenko@example.com",
            OrderDate = new DateTime(2025, 1, 15, 10, 30, 0, DateTimeKind.Utc),
            Status = "Shipped",
            Items = [new() { ProductId = 1, ProductName = "Wireless Mouse", Quantity = 2, UnitPrice = 24.99m }, new() { ProductId = 4, ProductName = "USB-C Hub", Quantity = 1, UnitPrice = 39.50m }]
        },
        new()
        {
            Id = 2,
            CustomerName = "Olena Kovalenko",
            CustomerEmail = "olena.kovalenko@example.com",
            OrderDate = new DateTime(2025, 2, 3, 14, 5, 0, DateTimeKind.Utc),
            Status = "Pending",
            Items = [new() { ProductId = 3, ProductName = "27\" 4K Monitor", Quantity = 1, UnitPrice = 329.00m }]
        }
    ];

    private readonly Lock _lock = new();
    private int _nextId = 3;

    public IReadOnlyList<Order> GetAll()
    {
        lock (_lock)
        {
            return [.. _orders];
        }
    }

    public Order? GetById(int id)
    {
        lock (_lock)
        {
            return _orders.FirstOrDefault(order => order.Id == id);
        }
    }

    public Order Create(Order order)
    {
        lock (_lock)
        {
            order.Id = _nextId++;
            if (order.OrderDate == default)
            {
                order.OrderDate = DateTime.UtcNow;
            }

            _orders.Add(order);
            return order;
        }
    }

    public bool Update(int id, Order updatedOrder)
    {
        lock (_lock)
        {
            var existingOrder = _orders.FirstOrDefault(order => order.Id == id);
            if (existingOrder is null)
            {
                return false;
            }

            existingOrder.CustomerName = updatedOrder.CustomerName;
            existingOrder.CustomerEmail = updatedOrder.CustomerEmail;
            existingOrder.OrderDate = updatedOrder.OrderDate;
            existingOrder.Status = updatedOrder.Status;
            existingOrder.Items = updatedOrder.Items;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _orders.RemoveAll(order => order.Id == id) > 0;
        }
    }
}
