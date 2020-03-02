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

namespace SS.Template.Application.ProductDetail
{
    public interface IProductDetailsService
    {
        Task<PaginatedResult<ProductDetailsModel>> GetPage(PaginatedQuery request);

        Task<ProductDetailsModel> Get(Guid id);

        Task Create(ProductDetailsModel productDetails);

        Task Update(Guid id, ProductDetailsModel productDetails);

        Task Delete(Guid id);
    }

    public class ProductDetailsService : IProductDetailsService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;
        private readonly IRepository _repository;

        public ProductDetailsService(IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator, IRepository repository)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
            _repository = repository;
        }

        public async Task<ProductDetailsModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<ProductDetails>(x => x.Id == id && x.Status == EnabledStatus.Enabled)
                .ProjectTo<ProductDetailsModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<ProductDetails>(id);
            }

            return result;
        }

        public async Task<PaginatedResult<ProductDetailsModel>> GetPage(PaginatedQuery request)
        {
            var query = _readOnlyRepository.Query<ProductDetails>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.Type.Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<ProductDetailsModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Type);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);
            return page;
        }

        public async Task Create(ProductDetailsModel ProductDetails)
        {
            var entity = _mapper.Map<ProductDetails>(ProductDetails);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();
        }

        public async Task Update(Guid id, ProductDetailsModel ProductDetails)
        {
            var entity = await _repository.FirstAsync<ProductDetails>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<ProductDetails>(id);
            }

            _mapper.Map(ProductDetails, entity);
            await _repository.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await _repository.FirstAsync<ProductDetails>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<ProductDetails>(id);
            }

            entity.Status = EnabledStatus.Deleted;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
