using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsApp.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Clothes"},
                    new Category { Id = 2, Name = "Food"},
                    new Category { Id = 3, Name = "Toys"}
                );
            builder.Entity<Product>().HasData(
                    new Product { Id = 1, Name = "T-Shirt", Price = 8.99, CategoryId  = 1},
                    new Product { Id = 2, Name = "Yogurt", Price = 1.99, CategoryId  = 2},
                    new Product { Id = 3, Name = "Ball", Price = 3.99, CategoryId  = 3}
                );
            base.OnModelCreating(builder);
        }
    }
}
