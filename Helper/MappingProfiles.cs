using AutoMapper;
using WebApi.Models;
using WepApi.Dto;

namespace WepApi.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
         .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
         .ForMember(dest => dest.PrimaryImageUrl, opt => opt.MapFrom(src =>
             src.Images.FirstOrDefault(i => i.IsPrimary) != null ?
             $"/api/product/{src.Id}/images/{src.Images.FirstOrDefault(i => i.IsPrimary).Id}" :
             null));

            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src =>
                    $"/api/product/{src.ProductId}/images/{src.Id}"));
            CreateMap<Orders, OrderDto>();
            CreateMap<Customer, CustomerDto>()
                 .ForMember(dest => dest.Orders,
                     opt => opt.MapFrom(src => src.Orders))
                 .ForMember(dest => dest.Products,
                     opt => opt.MapFrom(src => src.Orders.Select(o => o.Product).Distinct()))
                 .ForMember(dest => dest.Password,
                     opt => opt.Ignore()); // Typically we don't map passwords to DTOs

        }


    }
}
