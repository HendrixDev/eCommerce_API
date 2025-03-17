using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_API.Models
{
    public class Cart
    {
        public List<Product> ShoppingCart { get; set; } = new List<Product>();
        public decimal CartTotal { get; set; }
    }
}
