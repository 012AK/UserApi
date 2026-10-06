using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Services;

public class AssetService : IAssetService
{
    private readonly AppDbContext _context;

    public AssetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Asset>> GetAllAsync()
    {
        return await _context.Assets
            .OrderBy(a => a.Symbol)
            .ToListAsync();
    }

    public async Task<Asset?> GetByIdAsync(int id)
    {
        return await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Asset> CreateAsync(Asset asset)
    {
        _context.Assets.Add(asset);

        await _context.SaveChangesAsync();

        return asset;
    }

    public async Task DeleteAsync(int id)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == id);

        if (asset == null)
            throw new KeyNotFoundException("Asset not found");

        _context.Assets.Remove(asset);

        await _context.SaveChangesAsync();
    }
}