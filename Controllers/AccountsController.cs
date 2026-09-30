using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Services;
using UserApi.DTOs;

namespace UserApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount()
    {
        var userId = GetUserId();

        var account = await _accountService.CreateAccountAsync(userId);

        return Ok(account);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = GetUserId();

        var account = await _accountService.GetMyAccountAsync(userId);

        if (account == null)
            return NotFound(new
            {
                message = "Account not found."
            });

        return Ok(account);
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

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit(BalanceOperationDto dto)
    {
        var userId = GetUserId();

        var account = await _accountService
            .DepositAsync(userId, dto.Amount);

        if (account == null)
            return NotFound(new
            {
                message = "Account not found."
            });

        return Ok(account);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw(BalanceOperationDto dto)
    {
        var userId = GetUserId();

        var account = await _accountService
            .WithdrawAsync(userId, dto.Amount);

        if (account == null)
            return NotFound(new
            {
                message = "Account not found."
            });

        return Ok(account);
    }
}