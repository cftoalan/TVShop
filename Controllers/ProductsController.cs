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
                ViewData["CustomerId"] = HttpContext.Session.GetInt32("CustomerId");
                ViewData["CustomerName"] = HttpContext.Session.GetString("CustomerName");
            }
            else
            {
                ViewData["LoggedIn"] = "no";
            }

            var finalProjectContext = await _context.Televisions.Include(t => t.Manufacturer).ToListAsync();
            return View(finalProjectContext);
        }

        
        private bool TelevisionExists(int id)
        {
          return (_context.Televisions?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
