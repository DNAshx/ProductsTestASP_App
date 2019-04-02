using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductTestWork.Data.Entities;
using ProductTestWork.Data.Repositories;
using ProductTestWork.ViewModels;

namespace ProductTestWork.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;        

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;            
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
                searchString = currentFilter;
            }            

            int pageNumber = (page ?? 1);

            var productViewModel = new AllProductsViewModel(_productRepository, searchString, sortOrder, pageNumber);

            return View(productViewModel);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVM = new ProductViewModel(_productRepository, id);
            if (productVM.CurrentProduct == null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var productVM = new ProductViewModel(_productRepository);
            return View(productVM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "CurrentProduct")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _productRepository.SaveProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            
            return View(new ProductViewModel(_productRepository));
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productVM = new ProductViewModel(_productRepository, id);
            
            if (productVM.CurrentProduct == null)
            {
                return NotFound();
            }
            
            return View(productVM);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(Prefix = "CurrentProduct")] Product product)
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
            
            return View(new ProductViewModel(_productRepository, id));
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productVM = new ProductViewModel(_productRepository, id);
            
            if (productVM.CurrentProduct == null)
            {
                return NotFound();
            }

            return View(productVM);
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
