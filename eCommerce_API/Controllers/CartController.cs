using eCommerce_API.Business.Interfaces;
using eCommerce_API.Data;
using eCommerce_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(DataContext context, ICache cache) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly ICache _cache = cache;

        // POST: api/Cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> AddToCart(int userId, int productID)
        {
            //TODO: look into passing product object instead of Id to remove the additional DB call
            var product = await _context.Products.FindAsync(productID);

            if (product == null)
            {
                return NotFound();
            }

            var cacheData = _cache.GetData<Cart>(userId.ToString());

            //TODO: maybe flip if/else logic?
            //Cache was null, create new cart for user and cache it
            if (cacheData == null)
            {
                var cart = new Cart();
                cart.Products.Add(product);
                var expiryTime = DateTime.Now.AddMinutes(1);
                _cache.SetData<Cart>(userId.ToString(), cart, expiryTime);
                return cart;
            }

            //Data was cached, update it and recache
            else
            {
                var expiryTime = DateTime.Now.AddMinutes(1);
                cacheData.Products.Add(product);
                _cache.SetData<Cart>(userId.ToString(), cacheData, expiryTime);
                return cacheData;
            }
        }
    }
}
