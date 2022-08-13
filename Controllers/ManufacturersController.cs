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
    public class ManufacturersController : Controller
    {
        private readonly FinalProjectContext _context;

        public ManufacturersController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: Manufacturers
        public async Task<IActionResult> Index()
        {
              return _context.Manufacturers != null ? 
                          View(await _context.Manufacturers.ToListAsync()) :
                          Problem("Entity set 'FinalProjectContext.Manufacturers'  is null.");
        }

        // GET: Manufacturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Manufacturers == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ManufacturerId == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }


        private bool ManufacturerExists(int id)
        {
          return (_context.Manufacturers?.Any(e => e.ManufacturerId == id)).GetValueOrDefault();
        }
    }
}
