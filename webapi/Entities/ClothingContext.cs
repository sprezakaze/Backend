using Microsoft.EntityFrameworkCore;

namespace webapi.Entities
{
    public class ClothingContext:DbContext
    {
        public ClothingContext(DbContextOptions<ClothingContext> options):base(options)
        {
            
        }
        public DbSet<Clothing> Clothings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; } 
    }
}
