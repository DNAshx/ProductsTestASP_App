using Microsoft.AspNetCore.Mvc.Rendering;
using ProductTestWork.Data.Entities;
using ProductTestWork.Data.Repositories;

namespace ProductTestWork.ViewModels
{
    public class ProductViewModel : IProductViewModel
    {
        private IProductRepository _productRepository;

        public Product CurrentProduct { get; set; }

        public SelectList Categories { get; set; }

        public ProductViewModel(IProductRepository productRepository, int? productId = null)
        {
            _productRepository = productRepository;

            SetCurrentProduct(productId);
            SetCategories();
        }

        private void SetCategories()
        {
            Categories = new SelectList(_productRepository.GetAllProductCategories(), "Id", "CategoryType", CurrentProduct.CategoryId);
        }

        private void SetCurrentProduct(int? productId)
        {
            if (productId.HasValue)
            {
                CurrentProduct = _productRepository.GetProductById(productId ?? 0);
            }
            else
            {
                CurrentProduct = new Product();
            }
        }
    }
}
