namespace UserApi.DTOs;

public class PortfolioItemDto
{
    public string Symbol { get; set; } = "";
    public string AssetName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal TotalValue { get; set; }
}