using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Common;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Services;

namespace OnlineStore.WebApi.Controllers;

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

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<OrderResponse>> Checkout(CheckoutRequest request)
    {
        try
        {
            var userId = GetUserId();
            var order = await _orderService.CheckoutAsync(userId, request);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my")]
    public async Task<ActionResult<PagedResult<OrderResponse>>> GetMyOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        var orders = await _orderService.GetMyOrdersAsync(userId, pageNumber, pageSize);
        return Ok(orders);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResult<OrderResponse>>> GetAdminOrders([FromQuery] OrdersAdminQuery query)
    {
        var orders = await _orderService.GetAllOrdersAsync(query);
        return Ok(orders);
    }
}
