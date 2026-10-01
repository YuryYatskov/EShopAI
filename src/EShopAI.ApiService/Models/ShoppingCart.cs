namespace EShopAI.ApiService.Models;

public class ShoppingCart
{
    public string CustomerEmail { get; set; } = string.Empty;

    public List<CartItem> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(item => item.UnitPrice * item.Quantity);
}

public class CartItem
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
}

public record AddCartItemRequest(int ProductId, int Quantity = 1);

public record CheckoutRequest(string CustomerName);
