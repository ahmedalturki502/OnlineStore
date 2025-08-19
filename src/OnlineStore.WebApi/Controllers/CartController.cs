using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Interfaces.Services;

namespace OnlineStore.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetCart()
    {
        var userId = GetUserId();
        var cart = await _cartService.GetAsync(userId);
        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartResponse>> AddItem(AddCartItemRequest request)
    {
        try
        {
            var userId = GetUserId();
            var cart = await _cartService.AddAsync(userId, request);
            return Ok(cart);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("items/{productId}")]
    public async Task<ActionResult<CartResponse>> UpdateItem(Guid productId, UpdateCartItemRequest request)
    {
        try
        {
            var userId = GetUserId();
            var cart = await _cartService.UpdateAsync(userId, productId, request.Quantity);
            return Ok(cart);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("items/{productId}")]
    public async Task<IActionResult> RemoveItem(Guid productId)
    {
        var userId = GetUserId();
        await _cartService.RemoveAsync(userId, productId);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = GetUserId();
        await _cartService.ClearAsync(userId);
        return NoContent();
    }
}
