using UserApi.Models;

namespace UserApi.Services;

public interface IOrderService
{
    Task<Order> BuyAsync(
        int userId,
        int assetId,
        int quantity);

    Task<Order> SellAsync(
        int userId,
        int assetId,
        int quantity);

    Task<object> GetMyOrdersAsync(
        int userId,
        int page,
        int pageSize,
        string? type,
        string? sortBy,
        string? sortOrder);
}