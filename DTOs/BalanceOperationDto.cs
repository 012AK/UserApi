using System.ComponentModel.DataAnnotations;

namespace UserApi.DTOs;

public class BalanceOperationDto
{
    [Range(0.01, 1000000000)]
    public decimal Amount { get; set; }
}