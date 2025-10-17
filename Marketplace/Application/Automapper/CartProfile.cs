using AutoMapper;
using Marketplace.Application.Dto;
using Marketplace.Application.ServiceModels;
using Marketplace.Domain.DataModels;

namespace Marketplace.Application.Automapper
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