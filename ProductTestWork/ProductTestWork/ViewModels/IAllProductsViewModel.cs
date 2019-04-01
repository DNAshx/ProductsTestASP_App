using ProductTestWork.Data.Entities;
using ProductTestWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ProductTestWork.ViewModels
{
    public interface IAllProductsViewModel
    {
        string CurrentSort { get; set; }

        string IdSortParm { get; set; }

        string NameSortParm { get; set; }

        string CategorySortParm { get; set; }

        string CurrentFilter { get; set; }

        int PageSize { get; set; }

        int PageNumber { get; set; }

        List<ProductModel> ProductList { get; set; }

        IPagedList<ProductModel> ProductPagedList { get; }

        void SetProductsFromDb(List<Product> entities);
    }
}
