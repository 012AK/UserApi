using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.DTOs;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/accounts/{accountId}/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(
        int accountId,
        [FromBody] BalanceOperationDto dto)
    {
        var userId = GetUserId();

        await _transactionService.DepositAsync(
            userId,
            accountId,
            dto.Amount);

        return Ok(new
        {
            message = "Account successfully deposited"
        });
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(
        int accountId,
        [FromBody] BalanceOperationDto dto)
    {
        var userId = GetUserId();

        await _transactionService.WithdrawAsync(
            userId,
            accountId,
            dto.Amount);

        return Ok(new
        {
            message = "Withdrawal successful"
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetHistory(int accountId)
    {
        var userId = GetUserId();

        var transactions =
            await _transactionService.GetHistoryAsync(
                userId,
                accountId);

        return Ok(transactions);
    }

    private int GetUserId()
    {
        var userId = User.FindFirst(
            ClaimTypes.NameIdentifier
        )?.Value;

        if (userId == null)
            throw new UnauthorizedAccessException();

        return int.Parse(userId);
    }
}