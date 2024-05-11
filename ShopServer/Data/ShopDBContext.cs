using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedModels.Models;

namespace ShopServer.Data {
    public class ShopDBContext(DbContextOptions<ShopDBContext> options) : DbContext(options) {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
