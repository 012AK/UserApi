using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Models;

namespace UserApi.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task DepositAsync(int userId, int accountId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a =>
                a.Id == accountId &&
                a.UserId == userId);

        if (account == null)
            throw new KeyNotFoundException("Account not found");

        account.Balance += amount;

        var transaction = new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            Type = "Deposit"
        };

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();
    }

    public async Task WithdrawAsync(int userId, int accountId, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a =>
                a.Id == accountId &&
                a.UserId == userId);

        if (account == null)
            throw new KeyNotFoundException("Account not found");

        if (account.Balance < amount)
            throw new InvalidOperationException("Insufficient funds");

        account.Balance -= amount;

        var transaction = new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            Type = "Withdraw"
        };

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetHistoryAsync(
        int userId,
        int accountId)
    {
        var accountExists = await _context.Accounts
            .AnyAsync(a =>
                a.Id == accountId &&
                a.UserId == userId);

        if (!accountExists)
            throw new KeyNotFoundException("Account not found");

        return await _context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}