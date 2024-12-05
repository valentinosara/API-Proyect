using API.FurtnitureStore.Data;
using API.FurtnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FurtnitureStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly APIFurnitureStoreContext _context;
        public OrdersController(APIFurnitureStoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            return await _context.Orders.Include(o => o.OrderDetails).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Order order)
        {
            if (order == null) return BadRequest("Order can not be Null");
            if (order.OrderDetails == null) return BadRequest("Order should have at least 1 detail");

            await _context.Orders.AddAsync(order);
            await _context.OrderDetails.AddRangeAsync(order.OrderDetails);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Post", order.Id, order);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Order order)
        {
            if (order == null) return BadRequest();
            if (order.Id <= 0) return BadRequest();

            var existingOrder = await _context.Orders.Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existingOrder == null) return NotFound();

            existingOrder.OrderNumber = order.OrderNumber;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.ClientId = order.ClientId;
            existingOrder.DeliveryDate = order.DeliveryDate;

            _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);

            _context.Orders.Update(existingOrder);
            _context.OrderDetails.AddRange(order.OrderDetails);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Order order)
        {
            if (order == null) return NotFound();
            if (order.Id <= 0) return BadRequest();

            var existingOrder = await _context.Orders.Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if(existingOrder == null) return NotFound();

            _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
            _context.Orders.Remove(existingOrder);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
