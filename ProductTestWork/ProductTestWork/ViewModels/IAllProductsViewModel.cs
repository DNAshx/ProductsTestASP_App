using ProductTestWork.Models;
using System.Collections.Generic;

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
    }
}
