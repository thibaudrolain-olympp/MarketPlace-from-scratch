using AutoMapper;
using Marketplace.DataModels;
using Marketplace.ServiceModels;
using System.Data.Entity;

namespace Marketplace.Services
{
    public class CartService(MarketplaceDbContext _context, IMapper _mapper) : ICartService
    {
        public async Task<CartServiceModel> GetCartAsync(string userId, CancellationToken cancellationToken)
        {
            var cartEntity = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cartEntity == null)
            {
                cartEntity = new Cart { UserId = userId };
                _context.Carts.Add(cartEntity);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return _mapper.Map<CartServiceModel>(cartEntity);
        }

        public async Task<CartServiceModel> AddItemAsync(string userId, CartItemServiceModel item, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Items.Add(_mapper.Map<CartItem>(item));
            }

            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CartServiceModel>(cart);
        }

        public async Task RemoveItemAsync(string userId, int productId, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Items.Remove(item);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<CartServiceModel> UpdateItemAsync(string userId, int productId, int quantity, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null)
                return new CartServiceModel { UserId = userId };

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return _mapper.Map<CartServiceModel>(cart);

            if (quantity <= 0)
                cart.Items.Remove(item);
            else
                item.Quantity = quantity;

            await _context.SaveChangesAsync(cancellationToken);

            var updated = await _context.Carts
                .Include(c => c.Items)
                .FirstAsync(c => c.Id == cart.Id, cancellationToken);

            return _mapper.Map<CartServiceModel>(updated);
        }

        public async Task ClearCartAsync(string userId, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

            if (cart == null) return;

            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}