using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.DTOs;
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
        [FromBody] OrderRequestDto dto)
    {
        var userId = GetUserId();

        var order = await _orderService.BuyAsync(
            userId,
            dto.AssetId,
            dto.Quantity);

        return Ok(order);
    }

    [HttpPost("sell")]
    public async Task<IActionResult> Sell(
        [FromBody] OrderRequestDto dto)
    {
        var userId = GetUserId();

        var order = await _orderService.SellAsync(
            userId,
            dto.AssetId,
            dto.Quantity);

        return Ok(order);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders(
        int page = 1,
        int pageSize = 10,
        string? type = null,
        string? sortBy = null,
        string? sortOrder = "desc")
    {
        var userId = GetUserId();

        var orders = await _orderService
            .GetMyOrdersAsync(
                userId,
                page,
                pageSize,
                type,
                sortBy,
                sortOrder);

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