using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.Template.Application.Infrastructure;
using SS.Template.Application.Queries;
using SS.Template.Core.Exceptions;
using SS.Template.Core.Persistence;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Orders
{
    public interface IOrdersService
    {
        Task<PaginatedResult<OrdersModel>> GetPage(PaginatedQuery request);

        Task<OrdersModel> Get(Guid id);

        Task Create(OrdersModel orders);

        Task Update(Guid id, OrdersModel orders);

        Task Delete(Guid id);
    }

    public class OrdersService : IOrdersService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;
        private readonly IRepository _repository;

        public OrdersService(IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator, IRepository repository)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
            _repository = repository;
        }

        public async Task<OrdersModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<Cart>(x => x.Id == id && x.Status == EnabledStatus.Enabled)
                .ProjectTo<OrdersModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Cart>(id);
            }

            return result;
        }

        public async Task<PaginatedResult<OrdersModel>> GetPage(PaginatedQuery request)
        {
            var query = _readOnlyRepository.Query<Cart>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.DateCreated.ToString().Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<OrdersModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.DateCreated);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);
            return page;
        }

        public async Task Create(OrdersModel Order)
        {
            var entity = _mapper.Map<Cart>(Order);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task Update(Guid id, OrdersModel Order)
        {
            var entity = await _repository.FirstAsync<Cart>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Cart>(id);
            }

            _mapper.Map(Order, entity);
            await _repository.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _repository.FirstAsync<Cart>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Cart>(id);
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
