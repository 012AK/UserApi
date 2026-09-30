using UserApi.DTOs;

namespace UserApi.Services;

public interface IAccountService
{
    Task<AccountDto> CreateAccountAsync(int userId);

    Task<AccountDto?> GetMyAccountAsync(int userId);
    
    Task<AccountDto?> DepositAsync(int userId, decimal amount);

    Task<AccountDto?> WithdrawAsync(int userId, decimal amount);
}
