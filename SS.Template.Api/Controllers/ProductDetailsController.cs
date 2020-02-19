using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Template.Application.ProductDetail;
using SS.Template.Application.Queries;

namespace SS.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailsService _productDetailsService;

        public ProductDetailsController(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        // GET: api/productdetails
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<ProductDetailsModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginatedQuery query)
        {
            var page = await _productDetailsService.GetPage(query);
            return Ok(page);
        }

        // GET: api/productdetails/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductDetailsModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var productDetails = await _productDetailsService.Get(id);
            return Ok(productDetails);
        }

        // POST: api/productdetails
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] ProductDetailsModel productDetails)
        {
            await _productDetailsService.Create(productDetails);
            return Ok();
        }

        // PUT: api/productdetails/5
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDetailsModel productDetails)
        {
            await _productDetailsService.Update(id, productDetails);
            return Ok();
        }

        // DELETE: api/productdetails/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productDetailsService.Delete(id);
            return Ok();
        }
    }
}
