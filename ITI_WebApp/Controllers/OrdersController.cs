using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestRestApi.Data;
using TestRestApi.Data.Models;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db) 
        {
            _db = db;
        }
        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _db.Orders.Where(x => x.Id == orderId).FirstOrDefaultAsync();
            if(order != null)
            {
                dtoOrders dto = new()
                {
                    orderId = order.Id,
                    OrderDate = order.CreatedDate,
                };
                if(order.OrderItems.Any())
                {
                    foreach(var item in order.OrderItems)
                    {
                        dtoOrdersItems dtoItem = new()
                        {
                            itemId = item.items.Id,
                            itemName = item.items.Name,
                            Price = (decimal)item.items.Price,
                            quantity = 1

                        };
                        dto.items.Add(dtoItem);
                    }
                }
                return Ok(dto);
            }
            return NotFound($"The order Id {orderId} not exists");
        }
        [HttpGet("[action]/{itemId:int}")]
        public async Task<IActionResult> GetOrderItemById(int itemId)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders =_db.Orders.Where(x => x.Id == 1).FirstOrDefault().OrderItems.FirstOrDefault().items;
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] dtoOrders order)
        {
            //if(ModelState.IsValid)
            //{
                
            //    return Ok(order);
            //}
            //return BadRequest();
            if (ModelState.IsValid)
            {
                Order mdl = new()
                {
                    CreatedDate = order.OrderDate,
                    OrderItems = new List<OrderItem>()
                };
                foreach (var item in order.items)
                {
                    OrderItem dtoItems = new()
                    {
                        ItemId = item.itemId,
                        Price = item.Price,
                    };
                    mdl.OrderItems.Add(dtoItems);
                }
                await _db.Orders.AddAsync(mdl);
                await _db.SaveChangesAsync();
               
                return Ok(order);
            }
            return BadRequest();
        }
    }
}
