using AutoMapper;
using OnlineStore.Application.DTOs;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateProductRequest, Product>();
        
        // Category mappings
        CreateMap<Category, CategoryResponse>();
        CreateMap<CreateCategoryRequest, Category>();
        
        // Cart mappings
        CreateMap<CartItem, CartItemResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.Product.Price * src.Quantity));
        
        CreateMap<Cart, CartResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Items.Sum(i => i.Product.Price * i.Quantity)));
        
        // Order mappings
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name));
        
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}
