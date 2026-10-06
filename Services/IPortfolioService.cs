using UserApi.DTOs;

namespace UserApi.Services;

public interface IPortfolioService
{
    Task<List<PortfolioItemDto>> GetMyPortfolioAsync(int userId);
}