using Marketplace.Domain.DataModels;

namespace Marketplace.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<Cart> AddAsync(Cart cart, CancellationToken cancellationToken = default);
        Task UpdateAsync(Cart cart, CancellationToken cancellationToken = default);
        Task RemoveItemAsync(CartItem item, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}