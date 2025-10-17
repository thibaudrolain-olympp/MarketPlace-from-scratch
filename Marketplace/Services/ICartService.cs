using Marketplace.ServiceModels;

namespace Marketplace.Services
{
    public interface ICartService
    {
        Task<CartServiceModel> GetCartAsync(string userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Ajoute un item (ou augmente la quantité s'il existe déjà) et retourne le panier à jour.
        /// </summary>
        Task<CartServiceModel> AddItemAsync(string userId, CartItemServiceModel item, CancellationToken cancellationToken = default);

        /// <summary>
        /// Met à jour la quantité d'un item (si quantity <= 0, supprime l'item).
        /// </summary>
        Task<CartServiceModel> UpdateItemAsync(string userId, int productId, int quantity, CancellationToken cancellationToken = default);

        Task RemoveItemAsync(string userId, int productId, CancellationToken cancellationToken = default);

        Task ClearCartAsync(string userId, CancellationToken cancellationToken = default);
    }
}