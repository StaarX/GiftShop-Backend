using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Products
{
    public sealed class ProductsMapping : Profile
    {
        public ProductsMapping()
        {
            CreateMap<Product, ProductsModel>()
                .ReverseMap()
                .ForMember(x => x.Id, e => e.Ignore())
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore())
                .ForMember(x => x.ProductDetails, e => e.Ignore())
                .ForMember(x => x.ProductCatRelation, e => e.Ignore())

            ;
        }
    }
}
