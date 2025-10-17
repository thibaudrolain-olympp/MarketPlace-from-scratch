using Marketplace.Domain.DataModels;
using Marketplace.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Repositories
{
    public class CartRepository(MarketplaceDbContext _context) : ICartRepository
    {
        public async Task<Cart?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
        }

        public async Task<Cart> AddAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync(cancellationToken);
            return cart;
        }

        public async Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveItemAsync(CartItem item, CancellationToken cancellationToken = default)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}