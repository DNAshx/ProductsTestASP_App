using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductTestWork.Data;
using ProductTestWork.Data.Entities;
using System;
using X.PagedList;
using ProductTestWork.Models;
using ProductTestWork.Data.Repositories;
using ProductTestWork.ViewModels;

namespace ProductTestWork.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private IAllProductsViewModel _productViewModel;        

        public ProductsController(IProductRepository productRepository,
            IAllProductsViewModel productViewModel)
        {
            _productRepository = productRepository;
            _productViewModel = productViewModel;
        }

        // GET: Products
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            int pageSize = 3;

            _productViewModel.CurrentSort = sortOrder;            
            _productViewModel.IdSortParm = sortOrder == "ProductId"  ? "ProductId_desc" : "ProductId";
            _productViewModel.NameSortParm = sortOrder == "ProductName" ? "ProductName_desc" : "ProductName";
            _productViewModel.CategorySortParm = sortOrder == "ProductCategory" ? "ProductCategory_desc" : "ProductCategory";

            //switch to first page if search for product
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            _productViewModel.CurrentFilter = searchString;

            int pageNumber = (page ?? 1);

            _productViewModel.SetProductsFromDb(_productRepository.GetProductsByPage(pageSize, pageNumber));

            if (!String.IsNullOrEmpty(searchString))
            {
                _productViewModel.ProductList = _productViewModel.ProductList
                    .Where(s => s.ProductName.Contains(searchString))
                    .ToList();
            }

            switch (sortOrder)
            {
                case "ProductId":
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderBy(p => p.ProductId).ToList();
                    break;
                case "ProductId_desc":
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderByDescending(p => p.ProductId).ToList();
                    break;
                case "ProductName":
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderBy(p => p.ProductName).ToList();
                    break;
                case "ProductName_desc":
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderByDescending(p => p.ProductName).ToList();
                    break;
                case "ProductCategory":
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderBy(p => p.CategoryType).ToList();
                    break;
                case "ProductCategory_desc":
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderByDescending(p => p.CategoryType).ToList();
                    break;
                default:  // Name ascending 
                    _productViewModel.ProductList = _productViewModel.ProductList.OrderBy(p => p.ProductId).ToList();
                    break;
            }
            
            return View(_productViewModel);            
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByIdAsync(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryType"] = new SelectList(_productRepository.GetAllProductCategories(), "Id", "CategoryType");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.SaveProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryType"] = new SelectList(_productRepository.GetAllProductCategories(), "Id", "CategoryType", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByIdAsync(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryType"] = new SelectList(_productRepository.GetAllProductCategories(), "Id", "CategoryType", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateProductAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryType"] = new SelectList(_productRepository.GetAllProductCategories(), "Id", "CategoryType", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetProductByIdAsync(id ?? 0);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteProductById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _productRepository.GetProductById(id) != null;
        }
    }
}
