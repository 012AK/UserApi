using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyPortfolio()
    {
        var userId = GetUserId();

        var portfolio =
            await _portfolioService.GetMyPortfolioAsync(userId);

        return Ok(portfolio);
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