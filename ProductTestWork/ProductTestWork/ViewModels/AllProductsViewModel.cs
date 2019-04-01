using ProductTestWork.Data.Entities;
using ProductTestWork.Data.Repositories;
using ProductTestWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ProductTestWork.ViewModels
{
    public class AllProductsViewModel : IAllProductsViewModel
    {
        private IProductRepository _productRepository;

        public const string ID_SORT = "ProductId";
        public const string ID_SORT_DESC = "ProductId_desc";
        public const string NAME_SORT = "ProductName";
        public const string NAME_SORT_DESC = "ProductName_desc";
        public const string CATEGORY_SORT = "ProductCategory";
        public const string CATEGORY_SORT_DESC = "ProductCategory_desc";

        public string CurrentSort { get; set; }

        public string IdSortParm { get; set; } = ID_SORT;

        public string NameSortParm { get; set; } = NAME_SORT;

        public string CategorySortParm { get; set; } = CATEGORY_SORT;

        public string CurrentFilter { get; set; }

        public int PageSize { get; set; } = 3;

        public int PageNumber { get; set; } = 1;

        public List<ProductModel> ProductList { get; set; }

        public IPagedList<ProductModel> ProductPagedList
        {
            get
            {
                return ProductList.ToPagedList();
            }
        }

        public AllProductsViewModel(IProductRepository productRepository)
        {
            ProductList = new List<ProductModel>();

            _productRepository = productRepository;            
        }

        public void SetProductsFromDb(List<Product> entities)
        {
            var resultList = new List<ProductModel>();
            entities.ForEach(p => resultList.Add(Map(p)));
            ProductList = resultList;
        }

        public void AddProduct(Product entity)
        {
            if (entity != null)
                ProductList.Add(Map(entity));
        }

        private ProductModel Map(Product entity)
        {
            return new ProductModel
            {
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                CategoryType = entity.ProductCategory.CategoryType
            };
        }
    }
}
