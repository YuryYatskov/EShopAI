namespace EShopAI.ApiService.Models;

public class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = "Pending";

    public List<OrderItem> Items { get; set; } = [];

    public decimal TotalAmount => Items.Sum(item => item.UnitPrice * item.Quantity);
}

public class OrderItem
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
