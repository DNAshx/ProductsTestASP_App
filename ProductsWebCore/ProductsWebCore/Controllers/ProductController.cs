using Microsoft.AspNetCore.Mvc;
using ProductsWebCore.Data.Entities;
using ProductsWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductsWebCore.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data
                //var tests = new List<TestListModel>();

                var customerData = new List<ProductModel>()
                {
                    new ProductModel { ProductId = 1, ProductName = "Name" }
                };

                ////Search
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    customerData = GetAndSetTestListModelCache()
                //        .Where(t => t.Name.ToLower().Contains(searchValue.ToLower()))
                //        .AsQueryable();
                //}
                //else
                //{
                //    customerData = GetAndSetTestListModelCache()
                //        .AsQueryable();
                //}

                ////Sorting
                //if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //else if (!string.IsNullOrEmpty(sortColumnDirection) && string.IsNullOrEmpty(sortColumn))
                //{
                //    sortColumn = "Status";

                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}

                ////total number of rows count 
                //recordsTotal = customerData.Count();

                ////Paging 
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //data.ForEach(async t => await _testListViewModel.UpdateStatus(t));

                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}