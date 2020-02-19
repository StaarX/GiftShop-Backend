using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Template.Application.Orders;
using SS.Template.Application.Queries;

namespace SS.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // GET: api/orders
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<OrdersModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginatedQuery query)
        {
            var page = await _ordersService.GetPage(query);
            return Ok(page);
        }

        // GET: api/orders/guid
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrdersModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var orders = await _ordersService.Get(id);
            return Ok(orders);
        }

        // POST: api/orders
        //Admin Route
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] OrdersModel orders)
        {
            await _ordersService.Create(orders);
            return Ok();
        }

        // PUT: api/orderss/5
        //Admin Route
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrdersModel orders)
        {
            await _ordersService.Update(id, orders);
            return Ok();
        }

        // DELETE: api/orderss/5
        //Admin Route
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _ordersService.Delete(id);
            return Ok();
        }
    }
}
