using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order> BuyAsync(
        int userId,
        int assetId,
        int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (account == null)
            throw new KeyNotFoundException("Account not found");

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == assetId);

        if (asset == null)
            throw new KeyNotFoundException("Asset not found");

        var totalAmount = asset.Price * quantity;

        if (account.Balance < totalAmount)
            throw new InvalidOperationException("Insufficient funds");

        account.Balance -= totalAmount;

        var order = new Order
        {
            UserId = userId,
            AssetId = assetId,
            Type = "BUY",
            Quantity = quantity,
            Price = asset.Price,
            TotalAmount = totalAmount
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> SellAsync(
        int userId,
        int assetId,
        int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (account == null)
            throw new KeyNotFoundException("Account not found");

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == assetId);

        if (asset == null)
            throw new KeyNotFoundException("Asset not found");

        var boughtQuantity = await _context.Orders
            .Where(o =>
                o.UserId == userId &&
                o.AssetId == assetId &&
                o.Type == "BUY")
            .SumAsync(o => (int?)o.Quantity) ?? 0;

        var soldQuantity = await _context.Orders
            .Where(o =>
                o.UserId == userId &&
                o.AssetId == assetId &&
                o.Type == "SELL")
            .SumAsync(o => (int?)o.Quantity) ?? 0;

        var availableQuantity = boughtQuantity - soldQuantity;

        if (availableQuantity < quantity)
            throw new InvalidOperationException(
                "Insufficient asset quantity");

        var totalAmount = asset.Price * quantity;

        account.Balance += totalAmount;

        var order = new Order
        {
            UserId = userId,
            AssetId = assetId,
            Type = "SELL",
            Quantity = quantity,
            Price = asset.Price,
            TotalAmount = totalAmount
        };

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<List<Order>> GetMyOrdersAsync(int userId)
    {
        return await _context.Orders
            .Include(o => o.Asset)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }
}