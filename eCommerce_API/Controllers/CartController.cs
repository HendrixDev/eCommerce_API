using eCommerce_API.Data;
using eCommerce_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(DataContext context) : ControllerBase
    {

        private Cart _cart = new();
        private readonly DataContext _context = context;


        // POST: api/Cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cart>> AddToCart(int productID)
        {

            //TODO: update to use the same Cart object instead of creating new one each time (caching?)
            //Also look into passing product object instead of Id to remove the additional DB call
            var product = await _context.Products.FindAsync(productID);

            if (product == null)
            {
                return NotFound();
            }

            _cart.ShoppingCart.Add(product);

            return _cart;
        }


    }
}
