using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProductController(ApiDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("The product id is wrong");
            }

            return product;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("The product is wrong"); 
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult>Update(int id, Product product)
        {

            if (id != product.ProductID)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(product);
        }

    }
}
