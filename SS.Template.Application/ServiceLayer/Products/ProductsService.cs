using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SS.Template.Application.Infrastructure;
using SS.Template.Application.Queries;
using SS.Template.Core;
using SS.Template.Core.Exceptions;
using SS.Template.Core.Persistence;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Products
{
    public interface IProductsService
    {
        Task<PaginatedResult<ProductsListModel>> GetPage(PaginatedQuery request);
        Task<PaginatedResult<ProductsListModel>> GetPage(PaginatedQuery request, Guid categoryid);

        Task<ProductsModel> Get(Guid id);
        Task<PaginatedResult<Category>> GetCategories(PaginatedQuery request);

        Task Create(ProductsModel products);

        Task Update(Guid id, ProductsModel products);

        Task Delete(Guid id);
    }

    public class ProductsService : IProductsService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly IPaginator _paginator;
        private readonly IRepository _repository;
        private readonly IImageResizer _resizer;

        public ProductsService(IReadOnlyRepository readOnlyRepository, IMapper mapper, IPaginator paginator, IRepository repository, IImageResizer resizer)
        {
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _paginator = paginator;
            _repository = repository;
            _resizer = resizer;
        }

        public async Task<ProductsModel> Get(Guid id)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Id == id && x.Status == EnabledStatus.Enabled)
                .ProjectTo<ProductsModel>(_mapper.ConfigurationProvider);

            var queryCategory = _readOnlyRepository.Query<ProductCat>(x => x.Status == EnabledStatus.Enabled && x.ProductId==id, y=> y.Category).ToList();




            var result = await _readOnlyRepository.SingleAsync(query);

            if (result == null)
            {
                throw EntityNotFoundException.For<Product>(id);
            }


            if (result.ProductDetails != null)
            {
                result.ProductDetails = result.ProductDetails.Where(x=>x.Status==EnabledStatus.Enabled).ToList();
            }
            

            result.Categories = new List<Category>();

            queryCategory.ForEach(x => {
                result.Categories.Add(new Category{
                                        Id=x.Category.Id,
                                        Name=x.Category.Name,
                                        Description=x.Category.Description                 
                });
            });
            return result;
        }

        public async Task<PaginatedResult<ProductsListModel>> GetPage(PaginatedQuery request)
        {
            var query = _readOnlyRepository.Query<Product>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.Name.Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<ProductsListModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Name);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);

            return page;
        }


        public async Task<PaginatedResult<ProductsListModel>> GetPage(PaginatedQuery request,Guid categoryid)
        {
            var query = _readOnlyRepository.Query<ProductCat>(x => x.CategoryId==categoryid && x.Product.Status==EnabledStatus.Enabled, y=> y.Product);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.Product.Name.Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<ProductsListModel>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Name);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);

            return page;
        }


        private string ImageResize(string img, int height, int width)
        {
            string saveHeaders = img.Substring(0,22);
            string imageWithoutHeaders = img.Substring(23);

            var stream = new MemoryStream(Convert.FromBase64String(imageWithoutHeaders));
            var output = new MemoryStream();

            this._resizer.Resize(stream, output, height, width);

            string resizedImage = Convert.ToBase64String(output.GetBuffer());

            return saveHeaders+','+resizedImage;
        }


        public async Task Create(ProductsModel product)
        {
            var entity = _mapper.Map<Product>(product);

            entity.ImgSource=this.ImageResize(entity.ImgSource, 225, 225);

            _repository.Add(entity);

            await _repository.SaveChangesAsync();

            var productid = entity.Id;
            if (product.Categories!=null)
            {
                var categories = product.Categories.ToList();

                foreach (var cat in categories)
                {
                    _repository.Add(new ProductCat() { CategoryId = cat.Id, ProductId = productid });
                    await _repository.SaveChangesAsync();

                }
            }
            
        }

        public async Task Update(Guid id, ProductsModel product)
        {
            var entity = await _repository.FirstAsync<Product>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Product>(id);
            }
            _mapper.Map(product, entity);
            entity.ImgSource = this.ImageResize(product.ImgSource, 225, 225);

            await _repository.SaveChangesAsync();

            //Updating categories.
            bool validator = true;

            if (product.ProductCatRelation==null)
            {
                product.ProductCatRelation = new List<ProductCat>();
            }

            var dbCategories = _repository.Query<ProductCat>(x=>x.ProductId==id).ToList();
            var newCategories = new List<Category>();
            
            try
            {
                newCategories = product.Categories.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception have been thrown {e.StackTrace}");
                validator = false;
            }

            if (validator){
                if (dbCategories.Count == newCategories.Count)
                {
                    for (int i = 0; i < dbCategories.Count; i++)
                    {
                        if (dbCategories[i].CategoryId != newCategories[i].Id)
                        {
                            validator = false;
                            break;
                        }
                    }
                }
                else
                {
                    validator = false;
                }

                if (!validator)
                {
                    for (int i = 0; i < dbCategories.Count; i++)
                    {
                        var aux = dbCategories[i];
                        _repository.Remove(aux);
                        await _repository.SaveChangesAsync();
                    }
                    for (int i = 0; i < newCategories.Count; i++)
                    {
                        _repository.Add(new ProductCat() { CategoryId = newCategories[i].Id, ProductId = id });
                        await _repository.SaveChangesAsync();
                    }

                }
            }


            

            //Updating productDetails.
            validator = true;
            bool same = true;

            var dbDetails = _repository.Query<ProductDetails>(x => x.ProductId == id && x.Status==EnabledStatus.Enabled).ToList();
            var newDetails = new List<ProductDetails>();

            try
            {
                newDetails = product.ProductDetails.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception have been thrown {e.StackTrace}");
                validator = false;
            }

            if (dbDetails.Count<=0 && !validator)
            {
               return;
            }
            //Delete all product details case.
            if (dbDetails.Count > 0 && !validator)
            {
                for (int i = 0; i < dbDetails.Count; i++)
                {
                    dbDetails[i].Status = EnabledStatus.Deleted;
                    _repository.Update(dbDetails[i]);
                    await _repository.SaveChangesAsync();
                }

                return;
                    
            }

            if (dbDetails.Count==newDetails.Count)
            {
                for (int i = 0; i < dbDetails.Count; i++)
                {
                    if (!newDetails.Contains(dbDetails[i]))
                    {
                        same = false;
                        break;
                    }
                }
                //Updating if the list are with the same product details.
                if (same)
                {
                    for (int i = 0; i < dbDetails.Count; i++)
                    {
                        for (int y = 0; y < newDetails.Count; y++)
                        {
                            if (dbDetails[i].Id==newDetails[y].Id)
                            {
                                dbDetails[i].Price= newDetails[y].Price;
                                dbDetails[i].Type = newDetails[y].Type;
                                dbDetails[i].Availability = newDetails[y].Availability;
                                _repository.Update(dbDetails[i]);
                                await _repository.SaveChangesAsync();
                            }
                        }
                    }
                }

            }
            else
            {
                same = false;
            }

            //Not same items in the list case.
            if (!same)
            {
                //Deleting in case a item in the new details is not in the db details (Deleted).
                for (int i = 0; i < dbDetails.Count; i++)
                {
                    if (!newDetails.Contains(dbDetails[i]))
                    {
                        dbDetails[i].Status = EnabledStatus.Deleted;
                        _repository.Update(dbDetails[i]);
                        await _repository.SaveChangesAsync();
                    }
                }


                //Updating new details.
                bool changed = false;
                for (int i = 0; i < newDetails.Count; i++)
                {
                    //Searching for same items and updating them.
                    for (int y = 0; y < dbDetails.Count; y++)
                    {
                        if (dbDetails[y].Id == newDetails[i].Id)
                        {
                            dbDetails[y].Price = newDetails[i].Price;
                            dbDetails[y].Type = newDetails[i].Type;
                            dbDetails[y].Availability = newDetails[i].Availability;
                            _repository.Update(dbDetails[y]);
                            await _repository.SaveChangesAsync();
                            changed = true;
                            break;
                        }
                    }

                    //In case a item is not in the data base already is added to.
                    if (!changed)
                    {
                        _repository.Add(newDetails[i]);
                        await _repository.SaveChangesAsync();
                        changed = false;
                    }
                    else
                    {
                        changed = false;
                    }
                }
            }

        }


        public async Task Delete(Guid id)
        {
            var entity = await _repository.FirstAsync<Product>(x => x.Id == id);

            if (entity == null)
            {
                throw EntityNotFoundException.For<Product>(id);
            }
            entity.Status = EnabledStatus.Deleted;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Category>> GetCategories(PaginatedQuery request)
        {
            var query = _readOnlyRepository.Query<Category>(x => x.Status == EnabledStatus.Enabled);

            if (!string.IsNullOrEmpty(request.Term))
            {
                var term = request.Term.Trim();
                query = query.Where(x => x.Name.Contains(term));
            }

            var sortCriteria = request.GetSortCriteria();
            var items = query
                .ProjectTo<Category>(_mapper.ConfigurationProvider)
                .OrderByOrDefault(sortCriteria, x => x.Name);

            var page = await _paginator.MakePageAsync(_readOnlyRepository, query, items, request);
            return page;
        }
    }
}
