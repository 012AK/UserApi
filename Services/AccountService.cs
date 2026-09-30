using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.DTOs;
using UserApi.Models;

namespace UserApi.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _db;

    public AccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AccountDto> CreateAccountAsync(int userId)
    {
        var existingAccount = await _db.Accounts
            .FirstOrDefaultAsync(account => account.UserId == userId);

        if (existingAccount != null)
            throw new Exception("User already has an account.");

        var account = new Account
        {
            UserId = userId,
            Balance = 0,
            Currency = "KZT"
        };

        _db.Accounts.Add(account);

        await _db.SaveChangesAsync();

        return new AccountDto
        {
            Id = account.Id,
            Balance = account.Balance,
            Currency = account.Currency,
            CreatedAt = account.CreatedAt
        };
    }

    public async Task<AccountDto?> GetMyAccountAsync(int userId)
    {
        return await _db.Accounts
            .Where(account => account.UserId == userId)
            .Select(account => new AccountDto
            {
                Id = account.Id,
                Balance = account.Balance,
                Currency = account.Currency,
                CreatedAt = account.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AccountDto?> DepositAsync(int userId, decimal amount)
{
    var account = await _db.Accounts
        .FirstOrDefaultAsync(account => account.UserId == userId);

    if (account == null)
        return null;

    account.Balance += amount;

    await _db.SaveChangesAsync();

    return new AccountDto
    {
        Id = account.Id,
        Balance = account.Balance,
        Currency = account.Currency,
        CreatedAt = account.CreatedAt
    };
}

    public async Task<AccountDto?> WithdrawAsync(int userId, decimal amount)
    {
        var account = await _db.Accounts
            .FirstOrDefaultAsync(account => account.UserId == userId);

        if (account == null)
            return null;

        if (account.Balance < amount)
            throw new InvalidOperationException("Insufficient funds.");

        account.Balance -= amount;

        await _db.SaveChangesAsync();

        return new AccountDto
        {
            Id = account.Id,
            Balance = account.Balance,
            Currency = account.Currency,
            CreatedAt = account.CreatedAt
        };
    }
}