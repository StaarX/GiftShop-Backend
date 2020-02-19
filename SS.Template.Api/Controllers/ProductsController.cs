using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Template.Application.Products;
using SS.Template.Application.Queries;
using SS.Template.Domain.Entities;

namespace SS.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        // GET: api/products
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<ProductsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginatedQuery query)
        {
            var page = await _productsService.GetPage(query);
            return Ok(page);
        }

        // GET: api/products/categories
        [HttpGet("categories")]
        [ProducesResponseType(typeof(PaginatedResult<Category>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories([FromQuery] PaginatedQuery query)
        {
            var page = await _productsService.GetCategories(query);
            return Ok(page);
        }

        // GET: api/products/guid
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var products = await _productsService.Get(id);
            return Ok(products);
        }

        // POST: api/products
        //Admin Route
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] ProductsModel products)
        {
            await _productsService.Create(products);
            return Ok();
        }

        // PUT: api/products/5
        //Admin Route
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductsModel products)
        {
            await _productsService.Update(id, products);
            return Ok();
        }

        // DELETE: api/productss/5
        //Admin Route
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productsService.Delete(id);
            return Ok();
        }
    }
}
