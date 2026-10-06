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

    public async Task<object> GetMyOrdersAsync(
        int userId,
        int page,
        int pageSize,
        string? type,
        string? sortBy,
        string? sortOrder)
    {
        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        var query = _context.Orders
            .Include(o => o.Asset)
            .Where(o => o.UserId == userId);

        // Filter by BUY / SELL
        if (!string.IsNullOrWhiteSpace(type))
        {
            type = type.ToUpper();

            if (type != "BUY" && type != "SELL")
                throw new ArgumentException(
                    "Type must be BUY or SELL");

            query = query.Where(o => o.Type == type);
        }

        // Sorting
        sortBy = sortBy?.ToLower();
        sortOrder = sortOrder?.ToLower();

        if (sortBy == "price")
        {
            query = sortOrder == "asc"
                ? query.OrderBy(o => o.Price)
                : query.OrderByDescending(o => o.Price);
        }
        else if (sortBy == "quantity")
        {
            query = sortOrder == "asc"
                ? query.OrderBy(o => o.Quantity)
                : query.OrderByDescending(o => o.Quantity);
        }
        else
        {
            query = sortOrder == "asc"
                ? query.OrderBy(o => o.CreatedAt)
                : query.OrderByDescending(o => o.CreatedAt);
        }

        var totalCount = await query.CountAsync();

        var orders = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            page,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize),
            items = orders
        };
    }
}