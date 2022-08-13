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
    public class InvoicesController : Controller
    {
        private readonly FinalProjectContext _context;

        public InvoicesController(FinalProjectContext context)
        {
            _context = context;
        }

        

        // GET: Invoices/Create
        public async Task<IActionResult> Create(int CustomerId, int ProductId)
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


            Invoice invoice = new Invoice();
            var productdb = await _context.Televisions.Include(t => t.Manufacturer).FirstOrDefaultAsync(t => t.ProductId == ProductId);
            var customerdb = await _context.Customers.Include(i => i.Invoices).FirstOrDefaultAsync(c => c.CustomerId == CustomerId);
            invoice.CustomerId = CustomerId;
            invoice.ProductId = ProductId;
            invoice.Customer = customerdb;
            invoice.Date = DateTime.Today;
            invoice.Product = productdb;
            return View(invoice);
        }

        // POST: Invoices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int cid, int pid, Invoice invoice)
        {
            var productdb = await _context.Televisions.ToListAsync();
            var customerdb = await _context.Customers.ToListAsync();
            foreach (var product in productdb)
            {
                if (product.ProductId == pid)
                {
                    invoice.Product = product;
                }

            }
            foreach (var customer in customerdb)
            {
                if (customer.CustomerId == cid)
                {
                    invoice.Customer = customer;
                }
            }

            if (invoice.Quantity == null || invoice.Quantity == 0)
            {
                ModelState.AddModelError("QError", "Please enter the Quantity.");
            }

            if (invoice.Quantity < 0)
            {
                ModelState.AddModelError("QError", "Quantity must more than 1.");
            }

            if (invoice.Quantity > invoice.Product.Inventory)
            {
                ModelState.AddModelError("QError", "Sorry, we do not have enough Inventory right now.");
            }

            if (invoice.Date <= DateTime.Today)
            {
                ModelState.AddModelError("DError", "Please Choose the day after today.");
            }



            if (ModelState.IsValid)
            {
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                return RedirectToAction("AccountDetail", "Account");
            }
            return View(invoice);
        }


        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int id)
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

            var invdb = await _context.Invoices.Include(i => i.Customer).Include(i => i.Product).FirstOrDefaultAsync(i => i.InvoiceId == id);


            return View(invdb);
        }

        // POST: Invoices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Invoice invoice)
        {


            var productdb = await _context.Televisions.ToListAsync();
            var customerdb = await _context.Customers.ToListAsync();
            foreach (var product in productdb)
            {
                if (product.ProductId == invoice.ProductId)
                {
                    invoice.Product = product;
                }

            }
            foreach (var customer in customerdb)
            {
                if (customer.CustomerId == invoice.CustomerId)
                {
                    invoice.Customer = customer;
                }
            }

            if (invoice.Quantity > invoice.Product.Inventory)
            {
               ModelState.AddModelError("QError", "Sorry, we do not have enough Inventory right now.");
            }

            if (invoice.Quantity == null || invoice.Quantity == 0)
            {
                ModelState.AddModelError("QError", "Please enter the Quantity.");
            }

            if (invoice.Quantity < 0)
            {
                ModelState.AddModelError("QError", "Quantity must more than 1.");
            }

            if (invoice.Date <= DateTime.Today)
            {
                ModelState.AddModelError("DError", "Please Choose the day after today.");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("AccountDetail", "Account");
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        public async Task<IActionResult> Delete(int ProductId, int CustomerId)
        {
            
            var invoice = await _context.Invoices.FirstOrDefaultAsync(i=>i.CustomerId== CustomerId && i.ProductId==ProductId);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("AccountDetail", "Account");
        }

        public async Task<IActionResult> DeleteAll(int CustomerId)
        {

            var invoice = await _context.Invoices.Where(i=>i.CustomerId== CustomerId).ToListAsync();
            foreach (var cus in invoice)
            {
                if (cus.CustomerId==CustomerId)
                {
                    _context.Invoices.Remove(cus);
                }
            }
            

            await _context.SaveChangesAsync();
            return RedirectToAction("AccountDetail", "Account");
        }

        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
