using EShopAI.ApiService.Models;

namespace EShopAI.ApiService.Services;

public enum CartStatus
{
    Ok,
    ProductNotFound,
    InvalidQuantity,
    ItemNotFound,
    EmptyCart
}

public class CartService(ProductService products, OrderService orders)
{
    private readonly Dictionary<string, ShoppingCart> _carts = new(StringComparer.OrdinalIgnoreCase);
    private readonly Lock _lock = new();

    public ShoppingCart GetByEmail(string email)
    {
        lock (_lock)
        {
            return Copy(email, _carts.GetValueOrDefault(email));
        }
    }

    public (CartStatus Status, ShoppingCart? Cart) AddItem(string email, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            return (CartStatus.InvalidQuantity, null);
        }

        if (products.GetById(productId) is not { } product)
        {
            return (CartStatus.ProductNotFound, null);
        }

        lock (_lock)
        {
            if (!_carts.TryGetValue(email, out var cart))
            {
                cart = _carts[email] = new ShoppingCart { CustomerEmail = email };
            }

            if (cart.Items.FirstOrDefault(i => i.ProductId == productId) is { } existing)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = quantity
                });
            }

            return (CartStatus.Ok, Copy(email, cart));
        }
    }

    public bool RemoveItem(string email, int productId)
    {
        lock (_lock)
        {
            return _carts.TryGetValue(email, out var cart) && cart.Items.RemoveAll(i => i.ProductId == productId) > 0;
        }
    }

    public void Clear(string email)
    {
        lock (_lock)
        {
            _carts.Remove(email);
        }
    }

    public (CartStatus Status, Order? Order) Checkout(string email, string customerName)
    {
        lock (_lock)
        {
            if (!_carts.TryGetValue(email, out var cart) || cart.Items.Count == 0)
            {
                return (CartStatus.EmptyCart, null);
            }

            var order = orders.Create(new Order
            {
                CustomerName = customerName,
                CustomerEmail = email,
                Status = "Pending",
                Items = [.. cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                })]
            });

            _carts.Remove(email);
            return (CartStatus.Ok, order);
        }
    }

    private static ShoppingCart Copy(string email, ShoppingCart? cart) => new()
    {
        CustomerEmail = email,
        Items = [.. (cart?.Items ?? []).Select(i => new CartItem
        {
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity
        })]
    };
}
