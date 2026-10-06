using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("buy")]
    public async Task<IActionResult> Buy(
        int assetId,
        int quantity)
    {
        var userId = GetUserId();

        var order = await _orderService.BuyAsync(
            userId,
            assetId,
            quantity);

        return Ok(order);
    }

    [HttpPost("sell")]
    public async Task<IActionResult> Sell(
        int assetId,
        int quantity)
    {
        var userId = GetUserId();

        var order = await _orderService.SellAsync(
            userId,
            assetId,
            quantity);

        return Ok(order);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();

        var orders = await _orderService
            .GetMyOrdersAsync(userId);

        return Ok(orders);
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