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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // One-to-One relationship between User_detail and User_Other_Info based on Uid
      modelBuilder.Entity<User_detail>()
          .HasOne(ud => ud.UserOtherInfo)  // A User_detail has one related User_Other_Info
          .WithOne(uoi => uoi.UserDetail)  // A User_Other_Info has one related User_detail
          .HasForeignKey<User_Other_Info>(uoi => uoi.Uid) // Uid as foreign key in User_Other_Info
          .OnDelete(DeleteBehavior.Cascade) // Cascade delete if needed
          .IsRequired(); // Ensure Uid in User_Other_Info is required

      // One-to-Many relationship between User_detail and Order
      modelBuilder.Entity<User_detail>()
          .HasMany(ud => ud.Orders)    // One User_detail can have many Orders
          .WithOne(o => o.user)        // Each Order is related to one User_detail
          .HasForeignKey(o => o.Uid)   // Uid in Order is the foreign key
          .OnDelete(DeleteBehavior.Cascade)  // Cascade delete: If a User_detail is deleted, all related Orders will be deleted
          .IsRequired();                // Ensure that Uid is required in Order

      // Cascade update: Automatically update foreign keys in Orders when the primary key (Uid) in User_detail is updated
      modelBuilder.Entity<Order>()
          .HasOne(o => o.user)           // An Order is associated with one User_detail
          .WithMany(ud => ud.Orders)     // A User_detail can have many Orders
          .HasForeignKey(o => o.Uid)     // Uid is the foreign key in Order
          .OnDelete(DeleteBehavior.Cascade); // Optional: Ensure the relationship behaves as expected when the User_detail is deleted


      // One-to-One relationship between User_detail and Cart
      modelBuilder.Entity<User_detail>()
          .HasOne(u => u.Cart)          // A User_detail has one Cart
          .WithOne(c => c.user)         // A Cart is related to one User_detail
          .HasForeignKey<Cart>(c => c.Uid)   // Uid in Cart is the foreign key
          .OnDelete(DeleteBehavior.Cascade)  // Cascade delete: If User_detail is deleted, delete associated Cart
          .IsRequired();                // Ensure that a Cart must exist for each User_detail


      modelBuilder.Entity<Cart>()
        .HasMany(c => c.item)           // A Cart has many Items
        .WithOne(i => i.Cart)           // Each Item belongs to one Cart
        .HasForeignKey(i => i.CartId)   // CartId is the foreign key in Item
        .OnDelete(DeleteBehavior.Cascade); // Cascade delete: when a Cart is deleted, its related Items are deleted


      modelBuilder.Entity<Item>()
             .HasOne(i => i.Category)          // An Item has one Category
             .WithMany(c => c.category_items)  // A Category has many Items
             .HasForeignKey(i => i.Cid)        // Cid is the foreign key in Item
             .OnDelete(DeleteBehavior.Restrict); // Optionally add cascade delete behavior here if needed
    }
    // // One-to-Many relationship: Cart -> Item
    // modelBuilder.Entity<Cart>()
    //     .HasMany(c => c.item)  // A Cart has many Items
    //     .WithOne(i => i.Cart)   // Each Item belongs to one Cart
    //     .HasForeignKey(i => i.Cart) // Foreign key in Item pointing to Cart
    //     .OnDelete(DeleteBehavior.Cascade);  // Optional: Cascade delete behavior

    // // Additional configurations can go here
  }

}
