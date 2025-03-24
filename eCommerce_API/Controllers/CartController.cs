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


        // Get: api/Cart
        [HttpGet]
        public ActionResult<Cart> GetCart(string sessionId)
        {
            var cart = _cache.GetData<Cart>(sessionId);

            if (cart == null)
            {
                return new Cart();
            }

            else
            {
                cart.CartTotal = cart.Products.Select(x => x.Price).Sum();
                return cart;
            }
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<ActionResult<Cart>> AddToCart(string sessionId, int productID)
        {
            //TODO: look into passing product object instead of Id to remove the additional DB call
            var product = await _context.Products.FindAsync(productID);

            if (product == null)
            {
                return NotFound();
            }

            var cacheData = _cache.GetData<Cart>(sessionId);

            //Data was cached, update it and recache
            if (cacheData != null)
            {
                var expiryTime = DateTime.Now.AddMinutes(10);
                cacheData.Products.Add(product);
                _cache.SetData<Cart>(sessionId, cacheData, expiryTime);
                return cacheData;
            }

            //Cache was null, create new cart for user and cache it
            else
            {
                var cart = new Cart();
                cart.Products.Add(product);
                var expiryTime = DateTime.Now.AddMinutes(10);
                _cache.SetData<Cart>(sessionId, cart, expiryTime);
                return cart;
            }
        }
    }
}
