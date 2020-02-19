using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.ProductDetail
{
    public sealed class ProductDetailsMapping : Profile
    {
        public ProductDetailsMapping()
        {
            CreateMap<ProductDetails, ProductDetailsModel>()
                .ReverseMap()
                .ForMember(x => x.Id, e => e.Ignore())
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore());
        }
    }
}
