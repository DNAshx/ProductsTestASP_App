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
        private ProductContext _productContext;

        public ProductRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }

        public List<Product> GetAllProducts()
        {
            return _productContext.Products.Include(p => p.ProductCategory).ToList();            
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productContext.Products.Include(p => p.ProductCategory).ToListAsync();            
        }

        public Product GetProductById(int productId)
        {
            return _productContext.Products.Include(p => p.ProductCategory)
                    .FirstOrDefault(p => p.ProductId == productId);            
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productContext.Products.Include(p => p.ProductCategory)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public List<Product> GetProductsByPage(int pageSize, int pageNumber)
        {
            return _productContext.Products
                    .Include(p => p.ProductCategory)
                    .Skip(pageSize * (pageNumber - 1))
                    .Take(pageNumber)
                    .ToList();
        }

        public List<ProductCategory> GetAllProductCategories()
        {
            return _productContext.ProductCategories.ToList();            
        }

        public async Task SaveProductCategoryAsync(ProductCategory categoryToSave)
        {
            _productContext.Add(categoryToSave);
            await _productContext.SaveChangesAsync();            
        }

        public async Task SaveProductAsync(Product productToSave)
        {
            _productContext.Add(productToSave);
            await _productContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product productToUpdate)
        {
            _productContext.Update(productToUpdate);
            await _productContext.SaveChangesAsync();
        }

        public async Task DeleteProductById(int id)
        {
            var product = await GetProductByIdAsync(id);
            _productContext.Products.Remove(product);
            await _productContext.SaveChangesAsync();
        }
    }
}
