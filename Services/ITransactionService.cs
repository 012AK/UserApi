using UserApi.Models;

namespace UserApi.Services;

public interface ITransactionService
{
    Task DepositAsync(int userId, int accountId, decimal amount);

    Task WithdrawAsync(int userId, int accountId, decimal amount);

    Task<List<Transaction>> GetHistoryAsync(int userId, int accountId);
}