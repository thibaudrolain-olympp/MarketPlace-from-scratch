using AutoMapper;
using Marketplace.Application.Dto;
using Marketplace.Application.ServiceModels;
using Marketplace.Domain.DataModels;

namespace Marketplace.Application.Automapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryServiceModel, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryServiceModel>().ReverseMap();
        }
    }
}