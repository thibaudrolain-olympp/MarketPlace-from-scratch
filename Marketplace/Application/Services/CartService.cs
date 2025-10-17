using AutoMapper;
using Marketplace.Application.ServiceModels;
using Marketplace.Application.ServicesInterfaces;
using Marketplace.Domain.DataModels;
using Marketplace.Domain.Interfaces;

namespace Marketplace.Application.Services
{
    public class CartService(ICartRepository _cartRepo, IMapper _mapper) : ICartService
    {
        public async Task<CartServiceModel?> GetCartAsync(string userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId, cancellationToken);
            if (cart == null)
                return null;

            return _mapper.Map<CartServiceModel>(cart);
        }

        public async Task<CartServiceModel> AddItemAsync(string userId, CartItemServiceModel item, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId, cancellationToken)
                       ?? await _cartRepo.AddAsync(new Cart { UserId = userId }, cancellationToken);

            var existingItem = cart.Items.FirstOrDefault(i => i.Product.Id == item.ProductId);
            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
                cart.Items.Add(_mapper.Map<CartItem>(item));

            await _cartRepo.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CartServiceModel>(cart);
        }

        public async Task<CartServiceModel?> UpdateItemAsync(string userId, int productId, int quantity, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId, cancellationToken);
            if (cart == null) return null;

            var item = cart.Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null) return _mapper.Map<CartServiceModel>(cart);

            if (quantity <= 0)
                cart.Items.Remove(item);
            else
                item.Quantity = quantity;

            await _cartRepo.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CartServiceModel>(cart);
        }

        public async Task RemoveItemAsync(string userId, int productId, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId, cancellationToken);
            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
                await _cartRepo.RemoveItemAsync(item, cancellationToken);
        }

        public async Task ClearCartAsync(string userId, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepo.GetByUserIdAsync(userId, cancellationToken);
            if (cart == null) return;

            cart.Items.Clear();
            await _cartRepo.SaveChangesAsync(cancellationToken);
        }
    }
}