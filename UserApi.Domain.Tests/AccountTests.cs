using Xunit;
using UserApi.Domain;

namespace UserApi.Domain.Tests;

public class AccountTests
{
    [Fact]
    public void Deposit_ShouldIncreaseBalance()
    {
        var account = new Account();

        var result = account.Deposit(1000);

        Assert.True(result.IsSuccess);
        Assert.Equal(1000, account.Balance);
    }

    [Fact]
    public void Deposit_ShouldRejectNegativeAmount()
    {
        var account = new Account();

        var result = account.Deposit(-100);

        Assert.False(result.IsSuccess);
        Assert.Equal(0, account.Balance);
        Assert.Empty(account.Transactions);
    }

    [Fact]
    public void Withdraw_ShouldNotAllowMoreThanBalance()
    {
        var account = new Account();

        account.Deposit(500);

        var result = account.Withdraw(600);

        Assert.False(result.IsSuccess);
        Assert.Equal(500, account.Balance);
    }

    [Fact]
    public void Balance_ShouldEqualSumOfTransactions()
    {
        var account = new Account();

        account.Deposit(1000);
        account.Withdraw(300);
        account.Deposit(500);

        var transactionSum = account.Transactions
            .Sum(t => t.Amount);

        Assert.Equal(1200, account.Balance);
        Assert.Equal(account.Balance, transactionSum);
    }
}