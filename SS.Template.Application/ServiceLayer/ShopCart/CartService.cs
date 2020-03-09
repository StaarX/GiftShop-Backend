using System;
using System.Collections.Generic;
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

        Task Create(CartModel cart);
        Task insertItem(CartItemModel cartitem);

        Task updateQty(CartItemModel cartitem);
        Task<BoughtModel> buyTheCart(CartModel cart);
        Task<CartModel> Update(Guid id, CartModel cart);

        Task Delete(Guid idcart, Guid iddetail);
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

        public async Task insertItem(CartItemModel cartitem)
        {
            var query = _readOnlyRepository.Query<Cart>(x => x.UserId == cartitem.UserId && x.Status == EnabledStatus.Enabled)
               .ProjectTo<CartModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            var CartId = result.Id;

            foreach (var item in result.CartItems)
            {
                if (item.ProductDetailsId==cartitem.ProductDetail.Id)
                {
                    if ((item.Quantity+cartitem.Quantity)>cartitem.ProductDetail.Availability)
                    {
                        _repository.Update(new CartItem()
                        {
                            CartID = CartId,
                            ProductDetailsId = cartitem.ProductDetail.Id,
                            Quantity = item.ProductDetail.Availability,
                            UnitPrice = cartitem.UnitPrice
                        });
                        await _repository.SaveChangesAsync();
                        return;
                    }
                    else
                    {
                        _repository.Update(new CartItem()
                        {
                            CartID = CartId,
                            ProductDetailsId = cartitem.ProductDetail.Id,
                            Quantity = cartitem.Quantity+item.Quantity,
                            UnitPrice = cartitem.UnitPrice
                        });
                        await _repository.SaveChangesAsync();
                        return;
                    }
                }
            }
           
                _repository.Add(new CartItem()
                {
                    CartID = CartId,
                    ProductDetailsId = cartitem.ProductDetail.Id,
                    Quantity = cartitem.Quantity,
                    UnitPrice = cartitem.UnitPrice
                });
                await _repository.SaveChangesAsync();
            
        }
        public async Task<BoughtModel> buyTheCart(CartModel cart)
        {
            //Here should be handled availability and that type of things
            var query = _readOnlyRepository.Query<Cart>(x => x.UserId==cart.UserId && x.Status == EnabledStatus.Enabled)
               .ProjectTo<CartModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            var CartId = result.Id;

            var dbCartItems = _repository.Query<CartItem>(x => x.CartID == CartId, y => y.ProductDetail).ToList();

            var itemsThatApply = new List<CartItem>();
            var itemsThatDoesntApply = new List<CartItem>();

            dbCartItems.ForEach(dbitem =>
            {
                if (dbitem.Quantity > dbitem.ProductDetail.Availability || dbitem.Quantity < 0)
                {
                    itemsThatDoesntApply.Add(dbitem);
                }else if (dbitem.Quantity<=dbitem.ProductDetail.Availability)
                {
                    itemsThatApply.Add(dbitem);
                }
            });

            //We proceed to buy the items
                foreach (var item in itemsThatApply)
                {
                    item.ProductDetail.Availability = item.ProductDetail.Availability - item.Quantity;
                    _repository.Update(item.ProductDetail);
                    await _repository.SaveChangesAsync();
                    //Then the item gets deleted from cart
                    _repository.Remove(item);
                    await _repository.SaveChangesAsync();
                }



            return new BoughtModel()
            {
                itemsThatApplied = itemsThatApply,
                itemsThatDidntApply = itemsThatDoesntApply
            };
            //entity.Quantity = cartitem.Quantity;

            //_repository.Update(entity);

            //await _repository.SaveChangesAsync();

        }
        public async Task updateQty(CartItemModel cartitem)
        {
            var query = _readOnlyRepository.Query<Cart>(x => x.UserId == cartitem.UserId && x.Status == EnabledStatus.Enabled)
               .ProjectTo<CartModel>(_mapper.ConfigurationProvider);

            var result = await _readOnlyRepository.SingleAsync(query);

            var CartId = result.Id;

            var entity = await _repository.FirstAsync<CartItem>(x => x.ProductDetailsId == cartitem.ProductDetail.Id && x.CartID==CartId);

            entity.Quantity = cartitem.Quantity;

            _repository.Update(entity);

            await _repository.SaveChangesAsync();

        }



        public async Task<CartModel> Update(Guid id, CartModel cart)
        {
            var entity = await _repository.FirstAsync<Cart>(x => x.UserId == id);
            var cartID = entity.Id;
            if (entity == null)
            {
                throw EntityNotFoundException.For<Cart>(id);
            }
            var dbCartItems = _repository.Query<CartItem>(x => x.CartID == cartID,y=>y.ProductDetail).ToList();
            var frontCartItems = cart.CartItems.ToList();

            //If there is not items in the database cart, proceed to insert the items from front end directly.
            if (dbCartItems.Count<1)
            {
                foreach (var item in frontCartItems)
                {
                    _repository.Add(new CartItem() {
                                                   CartID=cartID,
                                                   ProductDetailsId=item.ProductDetail.Id,
                                                   Quantity=item.Quantity,
                                                   UnitPrice=item.UnitPrice
                                                   });
                    await _repository.SaveChangesAsync();
                }

                goto Finish;
            }
            //If the list is equal in lenght and items provided, proceed to update the items
            //in quantity desired
            var same = true;

            if (frontCartItems.Count==dbCartItems.Count)
            {
                frontCartItems.ForEach(fitem =>
                {
                    if (!dbCartItems.Contains(fitem))
                    {
                        same = false;
                    }
                });

                if (same)
                {
                    for (int i = 0; i < dbCartItems.Count; i++)
                    {
                        for (int y = 0; y < frontCartItems.Count; y++)
                        {
                            if (dbCartItems[i].Equals(frontCartItems[y]))
                            {
                                if ((dbCartItems[i].Quantity+frontCartItems[y].Quantity)>dbCartItems[i].ProductDetail.Availability)
                                {
                                    dbCartItems[i].Quantity = dbCartItems[i].ProductDetail.Availability;
                                    await _repository.SaveChangesAsync();
                                }
                                else
                                {
                                    dbCartItems[i].Quantity = dbCartItems[i].Quantity + frontCartItems[y].Quantity;
                                    await _repository.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    goto Finish;
                }
            }

            //If the list is not equal proceed to check which items aren't in the list and which items are in the list
            //In case a item is not in the list it will be added
            //In case a item is in the list it will be updated in quantity
            bool changed = false;
            for (int i = 0; i < frontCartItems.Count; i++)
            {
                //Searching for same items and updating them.
                for (int y = 0; y < dbCartItems.Count; y++)
                {
                    if (dbCartItems[y].Equals(frontCartItems[i]))
                    {
                        if ((dbCartItems[y].Quantity + frontCartItems[i].Quantity) > dbCartItems[y].ProductDetail.Availability)
                        {
                            dbCartItems[y].Quantity = dbCartItems[y].ProductDetail.Availability;
                        }
                        else
                        {
                            dbCartItems[y].Quantity = dbCartItems[y].Quantity + frontCartItems[i].Quantity;
                        }
                        _repository.Update(dbCartItems[y]);
                        await _repository.SaveChangesAsync();
                        changed = true;
                        break;
                    }
                }

                //In case a cartitem is not in the data base already is added to.
                if (!changed)
                {
                    _repository.Add(new CartItem()
                    {
                        CartID = cartID,
                        ProductDetailsId = frontCartItems[i].ProductDetail.Id,
                        Quantity = frontCartItems[i].Quantity,
                        UnitPrice = frontCartItems[i].UnitPrice
                    });
                    await _repository.SaveChangesAsync();
                    changed = false;
                }
                else
                {
                    changed = false;
                }
            }
            goto Finish;


            Finish:
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


        public async Task Delete(Guid idcart, Guid iddetail)
        {
            var entity = await _repository.FirstAsync<CartItem>(x => x.CartID==idcart && x.ProductDetailsId==iddetail);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Cart>(idcart);
            }

            _repository.Remove(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
