using AutoMapper;
using Marketplace.DataModels;
using Marketplace.Dto;
using Marketplace.ServiceModels;

namespace Marketplace.Automapper
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // DataModel <-> ServiceModel
            CreateMap<Cart, CartServiceModel>().ReverseMap();
            CreateMap<CartItem, CartItemServiceModel>().ReverseMap();

            // ServiceModel <-> DTO
            CreateMap<CartServiceModel, CartDto>().ReverseMap();
            CreateMap<CartItemServiceModel, CartItemDto>().ReverseMap();

            // Pour les requêtes spécifiques
            CreateMap<AddCartItemRequestDto, CartItemServiceModel>();
        }
    }
}