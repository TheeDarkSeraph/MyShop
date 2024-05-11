using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedModels.Models;
using ShopServer.Models;

namespace ShopServer.Data {
    public class ShopDBContext(DbContextOptions<ShopDBContext> options) : DbContext(options) {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<TokenInfo> Tokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserAccount>().Property(e => e.Role).HasConversion<short>();
        }
    }
}
