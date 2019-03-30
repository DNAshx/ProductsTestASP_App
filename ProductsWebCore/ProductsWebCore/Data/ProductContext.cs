using Microsoft.EntityFrameworkCore;
using ProductsWebCore.Data.Entities;

namespace ProductsWebCore.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext()
        {}

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductCategory)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<ProductCategory>().HasData(new ProductCategory
            {
                Id = 1,
                CategoryType = "Smarphones"
            },
            new ProductCategory
            {
                Id = 2,
                CategoryType = "TV"
            },
            new ProductCategory
            {
                Id = 3,
                CategoryType = "Laptops"
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
