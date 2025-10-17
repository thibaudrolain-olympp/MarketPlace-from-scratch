using AutoMapper;
using Marketplace.Dto;
using Marketplace.ServiceModels;
using Marketplace.Services;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController(ICartService _cartService, IMapper _mapper) : ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDto>> GetCart(string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            if (cart == null)
                return NotFound(); // 404 si l'utilisateur n'a pas de panier

            return Ok(_mapper.Map<CartDto>(cart));
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult<CartDto>> AddItem(string userId, [FromBody] AddCartItemRequestDto request)
        {
            var itemModel = _mapper.Map<CartItemServiceModel>(request);
            if (itemModel == null)
                throw new NullReferenceException("Mapping failed: CartItemServiceModel is null.");

            var updatedCart = await _cartService.AddItemAsync(userId, itemModel);
            return Ok(_mapper.Map<CartDto>(updatedCart));
        }

        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult<CartDto>> UpdateItem(string userId, int productId, [FromBody] UpdateCartItemRequestDto req, CancellationToken ct = default)
        {
            var updated = await _cartService.UpdateItemAsync(userId, productId, req.Quantity, ct);
            return Ok(_mapper.Map<CartDto>(updated));
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<IActionResult> RemoveItem(string userId, int productId)
        {
            await _cartService.RemoveItemAsync(userId, productId);
            return NoContent();
        }

        [HttpDelete("{userId}/items")]
        public async Task<IActionResult> Clear(string userId, CancellationToken ct = default)
        {
            await _cartService.ClearCartAsync(userId, ct);
            return NoContent();
        }
    }
}