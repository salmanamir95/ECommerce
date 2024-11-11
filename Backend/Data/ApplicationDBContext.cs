using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext (DbContextOptions dbContextOptions) : base (dbContextOptions)
        {

        }


        public DbSet<Cart> _cart { get; set; }

        public DbSet<Category> _category { get; set; }

        public DbSet<Item> _item { get; set; }

        public DbSet<Order> _order { get; set; }

        public DbSet<User_detail> _user_detail { get; set; }

        public DbSet<User_Other_Info> _user_other_info { get; set; }
    }
}
