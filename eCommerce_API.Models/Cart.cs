﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_API.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; } = [];
        public decimal CartTotal { get; set; }

        public decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var product in Products)
            {
                total += product.Price;
            }
            return total;
        }
    }
}
