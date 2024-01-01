using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestRestApi.Data;
using TestRestApi.Data.Models;
using TestRestApi.Models;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemsController(AppDbContext db)
        {
            _db = db;
            
        }
        private readonly AppDbContext _db;

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item =await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if(item == null)
            {
                return NotFound($"Item Id {id} not exists");
            }
            return Ok(item);
        }
        //Extension
        [HttpGet("ItemsWithCategory/{CategoryId}")]
        public async Task<IActionResult> GetAllItemsWithCategory(int CategoryId)
        {
            var item = await _db.Items.Where(x => x.CategoryId == CategoryId).ToListAsync();
            if (item == null)
            {
                return NotFound($"Category Id {CategoryId} has not items");
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddItem(MdlItem mdlItem)
        {
            using var stream = new MemoryStream();
            await mdlItem.Image.CopyToAsync(stream);
            var item = new Item
            {
                Name = mdlItem.Name,
                Price = mdlItem.Price,
                Notes = mdlItem.Notes,
                Image = stream.ToArray(),
                CategoryId = mdlItem.CategoryId
            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> updateItem(int id , MdlItem mdlItem)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound($"Item Id {id} not exists");
            }
            var isCategoryExists = await _db.Categories.AnyAsync(x => x.Id == mdlItem.CategoryId);
            if (isCategoryExists)
            {
                return NotFound($"category Id {mdlItem.CategoryId} not exists");
            }
            if(mdlItem.Image != null)
            {
                using var stream = new MemoryStream();
                await mdlItem.Image.CopyToAsync (stream);
                item.Image = stream.ToArray();
            }
            item.Name = mdlItem.Name;
            item.Price = mdlItem.Price;
            item.Notes = mdlItem.Notes;
            item.CategoryId = mdlItem.CategoryId;
            _db.SaveChanges();
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound($"Item Id {id} not exists");
            }
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }
    }
}
