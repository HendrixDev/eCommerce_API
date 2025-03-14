using eCommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerce_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
