using UserApi.Models;

namespace UserApi.Services;

public interface IAssetService
{
    Task<List<Asset>> GetAllAsync();

    Task<Asset?> GetByIdAsync(int id);

    Task<Asset> CreateAsync(Asset asset);

    Task DeleteAsync(int id);
}