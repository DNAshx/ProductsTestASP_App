using ProductTestWork.Data.Entities;
using ProductTestWork.Data.Repositories;
using ProductTestWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public string IdSortParm
        {
            get
            {
                return CurrentSort == ID_SORT ? ID_SORT_DESC : ID_SORT;
            }
        }

        public string NameSortParm
        {
            get
            {
                return CurrentSort == NAME_SORT ? NAME_SORT_DESC : NAME_SORT;
            }
        }

        public string CategorySortParm
        {
            get
            {
                return CurrentSort == CATEGORY_SORT ? CATEGORY_SORT_DESC : CATEGORY_SORT;
            }
        }

        public string CurrentFilter { get; set; }

        public int PageSize { get; set; } = 3;

        public int PageNumber { get; set; } = 1;

        public int PageCount { get; set; }

        public List<ProductModel> ProductList { get; set; }

        public AllProductsViewModel(IProductRepository productRepository,
            string searchString, string sortOrder, int pageNumber)
        {
            ProductList = new List<ProductModel>();

            _productRepository = productRepository;

            CurrentFilter = searchString;
            CurrentSort = sortOrder;
            PageNumber = pageNumber;

            SetProductsFromDb();
        }

        private void SetProductsFromDb()
        {            
            var entities = _productRepository.GetAllProducts();

            entities = FilterProductList(entities);
            entities = SortProductList(entities);
            PageCount = CalculatePageCount(entities);
            entities = GetPageItems(entities);

            var resultList = new List<ProductModel>();
            entities.ToList().ForEach(p => resultList.Add(Map(p)));
            ProductList = resultList;
        }

        public void AddProduct(Product entity)
        {
            if (entity != null)
                ProductList.Add(Map(entity));
        }

        private IQueryable<Product> FilterProductList(IQueryable<Product> products)
        {            
            if (!String.IsNullOrEmpty(CurrentFilter))
            {
                return products.Where(s => s.ProductName.Contains(CurrentFilter));
            }

            return products;
        }

        private IQueryable<Product> SortProductList(IQueryable<Product> products)
        {            
            switch (CurrentSort)
            {
                case ID_SORT:
                    return products.OrderBy(p => p.ProductId);                    
                case ID_SORT_DESC:
                    return products.OrderByDescending(p => p.ProductId);                    
                case NAME_SORT:
                    return products.OrderBy(p => p.ProductName);                    
                case NAME_SORT_DESC:
                    return products.OrderByDescending(p => p.ProductName);                    
                case CATEGORY_SORT:
                    return products.OrderBy(p => p.ProductCategory.CategoryType);                   
                case CATEGORY_SORT_DESC:
                    return products.OrderByDescending(p => p.ProductCategory.CategoryType);                    
                default:  // Name ascending 
                    return products.OrderBy(p => p.ProductId);                    
            }
        }

        private int CalculatePageCount(IQueryable<Product> products)
        {
            return (int)Math.Ceiling((double)products.Count() / PageSize);
        }

        private IQueryable<Product> GetPageItems(IQueryable<Product> products)
        {            
            return products.Skip(PageSize * (PageNumber - 1))
                    .Take(PageSize);
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
