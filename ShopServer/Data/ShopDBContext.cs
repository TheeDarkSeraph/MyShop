using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedModels.Models;

namespace ShopServer.Data {
    public class ShopDBContext:DbContext {
        public ShopDBContext(DbContextOptions<ShopDBContext> options) :base(options) {}
        public DbSet<Product> Products { get; set; }
    }
}
