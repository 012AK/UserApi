namespace UserApi.Models;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AssetId { get; set; }

    public string Type { get; set; } = "";

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public Asset Asset { get; set; } = null!;
}