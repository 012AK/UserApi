using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.DTOs;

namespace UserApi.Services;

public class PortfolioService : IPortfolioService
{
    private readonly AppDbContext _context;

    public PortfolioService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PortfolioItemDto>> GetMyPortfolioAsync(
        int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Asset)
            .Where(o => o.UserId == userId)
            .ToListAsync();

        var portfolio = orders
            .GroupBy(o => o.AssetId)
            .Select(group =>
            {
                var asset = group.First().Asset;

                var bought = group
                    .Where(o => o.Type == "BUY")
                    .Sum(o => o.Quantity);

                var sold = group
                    .Where(o => o.Type == "SELL")
                    .Sum(o => o.Quantity);

                var quantity = bought - sold;

                return new PortfolioItemDto
                {
                    Symbol = asset.Symbol,
                    AssetName = asset.Name,
                    Quantity = quantity,
                    CurrentPrice = asset.Price,
                    TotalValue = quantity * asset.Price
                };
            })
            .Where(x => x.Quantity > 0)
            .OrderBy(x => x.Symbol)
            .ToList();

        return portfolio;
    }
}