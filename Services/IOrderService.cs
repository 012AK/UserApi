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

    Task<List<Order>> GetMyOrdersAsync(int userId);
}