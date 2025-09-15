using AutoMapper;
using Marketplace.DataModels;
using Marketplace.Dto;
using Marketplace.ServiceModels;

namespace Marketplace.Automapper
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