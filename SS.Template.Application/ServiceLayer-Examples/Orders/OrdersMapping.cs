using AutoMapper;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Orders
{
    public sealed class OrdersMapping : Profile
    {
        public OrdersMapping()
        {
            CreateMap<Product, OrdersModel>()
                .ReverseMap()
                .ForMember(x => x.Id, e => e.Ignore())
                .ForMember(x => x.Status, e => e.Ignore())
                .ForMember(x => x.DateCreated, e => e.Ignore())
                .ForMember(x => x.DateUpdated, e => e.Ignore());
        }
    }
}
