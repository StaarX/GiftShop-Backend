using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Template.Application.ShopCart;
using SS.Template.Application.Queries;

namespace SS.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/cart
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<CartModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginatedQuery query)
        {
            var page = await _cartService.GetPage(query);
            return Ok(page);
        }

        // GET: api/cart/guid
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CartModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var cart = await _cartService.Get(id);
            return Ok(cart);
        }

        // POST: api/cart
        //Admin Route
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] CartModel cart)
        {
            await _cartService.Create(cart);
            return Ok();
        }

        // PUT: api/cart/5
        //Admin Route
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CartModel cart)
        {
            await _cartService.Update(id, cart);
            return Ok();
        }

        // DELETE: api/cart/5
        //Admin Route
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _cartService.Delete(id);
            return Ok();
        }
    }
}
