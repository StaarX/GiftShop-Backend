using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Products
{
    public sealed class ProductCatMapping : Profile
    {
        public ProductCatMapping()
        {
            CreateMap<ProductCat, ProductsListModel>()
                .ForMember(x => x.Id, e=> e.MapFrom(pc=>pc.ProductId))
                .ForMember(x => x.Name, e => e.MapFrom(pc => pc.Product.Name))
                .ForMember(x => x.Description, e => e.MapFrom(pc => pc.Product.Description))
                .ForMember(x => x.ImgSource, e => e.MapFrom(pc => pc.Product.ImgSource))
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore())
            ;
        }
    }
}
