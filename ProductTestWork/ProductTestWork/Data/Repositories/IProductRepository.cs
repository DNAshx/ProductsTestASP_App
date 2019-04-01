using ProductTestWork.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductTestWork.Data.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();

        Task<List<Product>> GetAllProductsAsync();

        Product GetProductById(int productId);

        Task<Product> GetProductByIdAsync(int productId);

        List<Product> GetProductsByPage(int pageSize, int pageNumber);

        List<ProductCategory> GetAllProductCategories();

        Task SaveProductCategoryAsync(ProductCategory categoryToSave);

        Task SaveProductAsync(Product productToSave);

        Task UpdateProductAsync(Product productToUpdate);

        Task DeleteProductById(int id);
    }
}
