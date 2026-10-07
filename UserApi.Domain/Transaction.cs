namespace UserApi.Domain;

public class Transaction
{
    public decimal Amount { get; }

    public string Type { get; }

    public DateTime CreatedAt { get; }

    public Transaction(
        decimal amount,
        string type)
    {
        Amount = amount;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }
}