using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.ShopCart
{
    public sealed class CartMapping : Profile
    {
        public CartMapping()
        {
            CreateMap<Cart, CartModel>()
                .ReverseMap()
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore());
        }
    }
}
