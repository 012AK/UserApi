namespace UserApi.DTOs;

public class AccountDto
{
    public int Id { get; set; }

    public decimal Balance { get; set; }

    public string Currency { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}