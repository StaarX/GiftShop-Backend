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

        [HttpPost("insertItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] CartItemModel cartitem)
        {
            await _cartService.insertItem(cartitem);
            return Ok();
        }

        [HttpPut("updateQty")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] CartItemModel cartitem)
        {
            await _cartService.updateQty(cartitem);
            return Ok();
        }

        [HttpPut("buyTheCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] CartModel cart)
        {
            var boughtmodel=await _cartService.buyTheCart(cart);
            return Ok(boughtmodel);
        }

        // PUT: api/cart/5
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CartModel cart)
        {
            var updatedcart = await _cartService.Update(id, cart);
            return Ok(updatedcart);
        }

        // DELETE: api/cart/5
        //Admin Route
        [HttpDelete("{idcart:guid}/{iddetail:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid idcart, Guid iddetail)
        {
            await _cartService.Delete(idcart,iddetail);
            return Ok();
        }
    }
}
