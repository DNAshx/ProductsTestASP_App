using Microsoft.AspNetCore.Mvc.Rendering;
using ProductTestWork.Data.Entities;

namespace ProductTestWork.ViewModels
{
    public interface IProductViewModel
    {
        Product CurrentProduct { get; set; }

        SelectList Categories { get; set; }
    }
}