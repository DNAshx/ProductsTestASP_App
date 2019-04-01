using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductTestWork.Data.Entities;
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
            //switch to first page if search for product
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = _productViewModel.CurrentFilter;
            }            

            int pageNumber = (page ?? 1);

            _productViewModel.SetProductsFromDb(_productRepository.GetAllProducts());

            _productViewModel.FilterProductList(searchString);

            _productViewModel.SortProductList(sortOrder);

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
