using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsWebCore.Data;
using ProductsWebCore.Data.Entities;
using System;
using X.PagedList;

namespace ProductsWebCore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        // GET: Products
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "ProductId" : "";
            ViewBag.IdSortParm = sortOrder == "ProductId"  ? "ProductId_desc" : "ProductId";
            ViewBag.NameSortParm = sortOrder == "ProductName" ? "ProductName_desc" : "ProductName";
            ViewBag.DateSortParm = sortOrder == "ProductCategory" ? "ProductCategory_desc" : "ProductCategory";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var products = from p in _context.Products.Include(p => p.ProductCategory)
                           select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ProductId":
                    products = products.OrderBy(p => p.ProductId);
                    break;
                case "ProductId_desc":
                    products = products.OrderByDescending(p => p.ProductId);
                    break;
                case "ProductName":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "ProductName_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "ProductCategory":
                    products = products.OrderBy(p => p.ProductCategory.CategoryType);
                    break;
                case "ProductCategory_desc":
                    products = products.OrderByDescending(p => p.ProductCategory.CategoryType);
                    break;
                default:  // Name ascending 
                    products = products.OrderBy(p => p.ProductId);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));            
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryType"] = new SelectList(_context.ProductCategories, "Id", "CategoryType");
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
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryType"] = new SelectList(_context.ProductCategories, "Id", "CategoryType", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryType"] = new SelectList(_context.ProductCategories, "Id", "CategoryType", product.CategoryId);
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
                    _context.Update(product);
                    await _context.SaveChangesAsync();
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
            ViewData["CategoryType"] = new SelectList(_context.ProductCategories, "Id", "CategoryType", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);
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
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
