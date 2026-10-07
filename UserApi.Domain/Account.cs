namespace UserApi.Domain;

public class Account
{
    private readonly List<Transaction> _transactions = [];

    public decimal Balance { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions =>
        _transactions.AsReadOnly();

    public Result Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Failure(
                "Deposit amount must be greater than 0");
        }

        Balance += amount;

        _transactions.Add(
            new Transaction(
                amount,
                "Deposit"));

        return Result.Success();
    }

    public Result Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            return Result.Failure(
                "Withdrawal amount must be greater than 0");
        }

        if (amount > Balance)
        {
            return Result.Failure(
                "Insufficient funds");
        }

        Balance -= amount;

        _transactions.Add(
            new Transaction(
                -amount,
                "Withdraw"));

        return Result.Success();
    }
}