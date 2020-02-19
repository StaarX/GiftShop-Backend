using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Products
{
    public sealed class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, Category>()
                .ReverseMap()
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore())
                .ForMember(x => x.ProductCatRelation,e=>e.Ignore());
            ;
        }
    }
}
