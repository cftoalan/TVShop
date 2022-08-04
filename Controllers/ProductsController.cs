using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TVShop.DataAccess;

namespace TVShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FinalProjectContext _context;

        public ProductsController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("LoggedIn") == "yes")
            {
                ViewData["LoggedIn"] = "yes";
                ViewData["CustomerId"] = HttpContext.Session.GetString("CustomerId");
                ViewData["CustomerName"] = HttpContext.Session.GetString("CustomerName");
            }
            else
            {
                ViewData["LoggedIn"] = "no";
            }

            var finalProjectContext = await _context.Televisions.Include(t => t.Manufacturer).ToListAsync();
            return View(finalProjectContext);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Televisions == null)
            {
                return NotFound();
            }

            var television = await _context.Televisions
                .Include(t => t.Manufacturer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (television == null)
            {
                return NotFound();
            }

            return View(television);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Model,ManufacturerId,Resolution,HdrSupport,ScreenSize,Price,Inventory")] Television television)
        {
            if (ModelState.IsValid)
            {
                _context.Add(television);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId", television.ManufacturerId);
            return View(television);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Buy(int? ProductId, int? CustomerId)
        {
            if (ProductId == null || _context.Televisions == null)
            {
                return NotFound();
            }

            var television = await _context.Televisions.FindAsync(ProductId);
            if (television == null)
            {
                return NotFound();
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId", television.ManufacturerId);
            return View(television);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Model,ManufacturerId,Resolution,HdrSupport,ScreenSize,Price,Inventory")] Television television)
        {
            if (id != television.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(television);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelevisionExists(television.ProductId))
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
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerId", television.ManufacturerId);
            return View(television);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Televisions == null)
            {
                return NotFound();
            }

            var television = await _context.Televisions
                .Include(t => t.Manufacturer)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (television == null)
            {
                return NotFound();
            }

            return View(television);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Televisions == null)
            {
                return Problem("Entity set 'FinalProjectContext.Televisions'  is null.");
            }
            var television = await _context.Televisions.FindAsync(id);
            if (television != null)
            {
                _context.Televisions.Remove(television);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelevisionExists(int id)
        {
          return (_context.Televisions?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
