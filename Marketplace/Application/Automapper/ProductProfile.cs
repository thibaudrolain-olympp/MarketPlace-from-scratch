using AutoMapper;
using Marketplace.Application.Dto;
using Marketplace.Application.ServiceModels;
using Marketplace.Domain.DataModels;

namespace Marketplace.Application.Automapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductServiceModel, ProductDto>().ReverseMap();
            CreateMap<Product, ProductServiceModel>().ReverseMap();
        }
    }
}