using Microsoft.EntityFrameworkCore;
using ProductTestWork.Data;
using ProductTestWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductTestWork.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public List<Product> GetAllProducts()
        {
            using (var context = new ProductContext())
            {
                return context.Products.Include(p => p.ProductCategory).ToList();
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            using (var context = new ProductContext())
            {
                return await context.Products.Include(p => p.ProductCategory).ToListAsync();
            }
        }

        public Product GetProductById(int productId)
        {
            using (var context = new ProductContext())
            {
                return context.Products.Include(p => p.ProductCategory)
                    .FirstOrDefault(p => p.ProductId == productId);
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            using (var context = new ProductContext())
            {
                return await context.Products.Include(p => p.ProductCategory)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
            }
        }

        public List<Product> GetProductsByPage(int pageSize, int pageNumber)
        {
            using (var context = new ProductContext())
            {
                return context.Products
                    .Include(p => p.ProductCategory)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageNumber)
                    .ToList();
            }
        }

        public List<ProductCategory> GetAllProductCategories()
        {
            using (var context = new ProductContext())
            {
                return context.ProductCategories.ToList();
            }
        }

        public async Task SaveProductCategoryAsync(ProductCategory categoryToSave)
        {
            using (var context = new ProductContext())
            {
                context.Add(categoryToSave);
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveProductAsync(Product productToSave)
        {
            using (var context = new ProductContext())
            {
                context.Add(productToSave);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateProductAsync(Product productToUpdate)
        {
            using (var context = new ProductContext())
            {
                context.Update(productToUpdate);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteProductById(int id)
        {
            using (var context = new ProductContext())
            {
                var product = await GetProductByIdAsync(id);
                context.Products.Remove(product);
                await context.SaveChangesAsync();
            }
        }
    }
}
