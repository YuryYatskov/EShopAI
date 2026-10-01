namespace EShopAI.Web.Services;

public class CartState
{
    public string Email { get; private set; } = string.Empty;

    public int ItemCount { get; private set; }

    public event Action? OnChange;

    public void SetEmail(string email)
    {
        Email = email.Trim();
        OnChange?.Invoke();
    }

    public void SetItemCount(int count)
    {
        ItemCount = count;
        OnChange?.Invoke();
    }
}
