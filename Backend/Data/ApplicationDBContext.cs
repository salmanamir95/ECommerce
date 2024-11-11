using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> dbContextOptions) : base(dbContextOptions) { }

        // DbSets representing the tables in your database
        public DbSet<Cart> _cart { get; set; }
        public DbSet<Category> _category { get; set; }
        public DbSet<Item> _item { get; set; }
        public DbSet<Order> _order { get; set; }
        public DbSet<User_detail> _user_detail { get; set; }
        public DbSet<User_Other_Info> _user_other_info { get; set; }



    }
}
