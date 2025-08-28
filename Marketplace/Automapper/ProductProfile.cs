using Marketplace.DataModels;
using Marketplace.ServiceModels;
using AutoMapper;

namespace Marketplace.Automapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
