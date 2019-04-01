using ProductTestWork.Data.Entities;
using ProductTestWork.Data.Repositories;
using ProductTestWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void FilterProductList(string filterString)
        {
            CurrentFilter = filterString;
            if (!String.IsNullOrEmpty(CurrentFilter))
            {
                ProductList = ProductList
                    .Where(s => s.ProductName.Contains(CurrentFilter))
                    .ToList();
            }
        }

        public void SortProductList(string sort = ID_SORT)
        {
            CurrentSort = sort;
            switch (CurrentSort)
            {
                case ID_SORT:
                    ProductList = ProductList.OrderBy(p => p.ProductId).ToList();
                    break;
                case ID_SORT_DESC:
                    ProductList = ProductList.OrderByDescending(p => p.ProductId).ToList();
                    break;
                case NAME_SORT:
                    ProductList = ProductList.OrderBy(p => p.ProductName).ToList();
                    break;
                case NAME_SORT_DESC:
                    ProductList = ProductList.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case CATEGORY_SORT:
                    ProductList = ProductList.OrderBy(p => p.CategoryType).ToList();
                    break;
                case CATEGORY_SORT_DESC:
                    ProductList = ProductList.OrderByDescending(p => p.CategoryType).ToList();
                    break;
                default:  // Name ascending 
                    ProductList = ProductList.OrderBy(p => p.ProductId).ToList();
                    break;
            }
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
