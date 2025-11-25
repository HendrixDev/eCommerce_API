using eCommerce_API.Business.Interfaces;
using eCommerce_API.Data;
using eCommerce_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController(DataContext context, ICache cache) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly ICache _cache = cache;
        private readonly DateTime _expiryTime = DateTime.Now.AddMinutes(10);

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
                return cart;
            }
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<ActionResult<Cart>> AddToCart(string sessionId, int productID, int quantity)
        {
            //TODO: look into passing product object instead of Id to remove the additional DB call
            var product = await _context.Products.FindAsync(productID);

            if (product == null)
            {
                return NotFound();
            }

            var cachedCart = _cache.GetData<Cart>(sessionId);

            //if cachedCart is null, create a new one
            Cart cart = cachedCart ?? new Cart();

            for (int i = 0; i < quantity; i++)
            {
                cart.Products.Add(product);
            }
            _cache.SetData<Cart>(sessionId, cart, _expiryTime);
            return cart;
        }

        [HttpPost]
        public ActionResult<Cart> IncreaseQuantity(string sessionId, int productID)
        {
            var cart = _cache.GetData<Cart>(sessionId);

            //get matching product from cart
            var productToAdd = cart.Products.First(x => x.ProductId == productID);

            cart.Products.Add(productToAdd);

            _cache.SetData<Cart>(sessionId, cart, _expiryTime);
            return cart;
        }


        [HttpDelete]
        public ActionResult<Cart> RemoveFromCart(string sessionId, int productId)
        {
            //get existing cart
            var cart = _cache.GetData<Cart>(sessionId);

            //remove products with matching IDs
            cart.Products = [.. cart.Products.Where(x => x.ProductId != productId)];

            if (cart.Products.Count == 0)
            {   //remove cart from cache if no products are left
                _cache.RemoveData(sessionId);
                return new Cart();
            }

            else
            {   //recache data
                _cache.SetData<Cart>(sessionId, cart, _expiryTime);
                return cart;
            }
        }
    }
}
