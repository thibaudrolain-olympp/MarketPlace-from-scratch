using AutoMapper;
using Marketplace.Application.Dto;
using Marketplace.Application.ServiceModels;
using Marketplace.Domain.DataModels;

namespace Marketplace.Application.Automapper
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile()
        {
            CreateMap<ProductImageServiceModel, ProductImageDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageServiceModel>().ReverseMap();
        }
    }
}