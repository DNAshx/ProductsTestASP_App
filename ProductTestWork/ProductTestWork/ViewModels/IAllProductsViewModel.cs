using ProductTestWork.Data.Entities;
using ProductTestWork.Models;
using System.Collections.Generic;
using X.PagedList;

namespace ProductTestWork.ViewModels
{
    public interface IAllProductsViewModel
    {
        string CurrentSort { get; set; }

        string IdSortParm { get; }

        string NameSortParm { get; }

        string CategorySortParm { get; }

        string CurrentFilter { get; set; }

        int PageSize { get; set; }

        int PageNumber { get; set; }

        List<ProductModel> ProductList { get; set; }

        IPagedList<ProductModel> ProductPagedList { get; }

        void SetProductsFromDb(List<Product> entities);

        void SortProductList(string sort);

        void FilterProductList(string filterString);
    }
}
