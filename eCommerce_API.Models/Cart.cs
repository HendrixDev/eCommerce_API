namespace eCommerce_API.Models
{
    public class Cart
    {
        public List<Product> Products { get; set; } = [];
        public decimal CartTotal => CalculateTotal();

        private decimal CalculateTotal()
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
