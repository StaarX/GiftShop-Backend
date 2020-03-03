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

namespace SS.Template.Application.ShopCart
{
    public interface ICartService
    {
        Task<PaginatedResult<CartModel>> GetPage(PaginatedQuery request);

        Task<CartModel> Get(Guid id);

        Task Create(CartModel orders);

        Task Update(Guid id, CartModel orders);

        Task Delete(Guid id);
    }

    public class CartService : ICartService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;
        private readonly IRepository _repository;

        public CartService(IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator, IRepository repository)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
            _repository = repository;
        }
        //Get cart by UserId
        public async Task<CartModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<Cart>(x => x.UserId == id && x.Status == EnabledStatus.Enabled)
                .ProjectTo<CartModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            var CartId = result.Id;

            result.CartItems = _readOnlyRepository.Query<CartItem>(x => x.CartID == CartId && x.Status == EnabledStatus.Enabled, i => i.ProductDetail.Product).ToList();

            if (result == null)
            {
                throw EntityNotFoundException.For<Cart>(id);
            }

            return result;
        }
       
        public async Task<PaginatedResult<CartModel>> GetPage(PaginatedQuery request)
        {
            var query = _readOnlyRepository.Query<Cart>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.DateCreated.ToString().Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<CartModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.DateCreated);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);
            return page;
        }

        //This should be used when a new user signs up
        public async Task Create(CartModel cart)
        {
            var entity = _mapper.Map<Cart>(cart);

            _repository.Add(entity);
            await _repository.SaveChangesAsync();
            if (cart.CartItems != null)
            {
                var cartitemslist=cart.CartItems.ToList();

                foreach (var item in cart.CartItems)
                {
                    _repository.Add(new CartItem() {ProductDetailsId=item.ProductDetailsId,
                                                    CartID=item.CartID,
                                                    Quantity=item.Quantity,
                                                    UnitPrice=item.ProductDetail.Price,

                                                });
                    await _repository.SaveChangesAsync();
                }
            }

        }



        public async Task Update(Guid id, CartModel cart)
        {
            var entity = await _repository.FirstAsync<Cart>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Cart>(id);
            }

            _mapper.Map(cart, entity);
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
