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
    public class CustomersController : Controller
    {
        private readonly FinalProjectContext _context;

        public CustomersController(FinalProjectContext context)
        {
            _context = context;
        }


        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            var cusdb = await _context.Customers.ToListAsync();
            foreach (var cn in cusdb)
            {
                if(customer.Name==cn.Name)
                {
                    ModelState.AddModelError("NError", "This user name already used");
                }
            }
            
            if (customer.Name == null)
            {
                ModelState.AddModelError("NError", "User name is required");
            }
            else
            {
                if (customer.Name.Length < 3)
                {
                    ModelState.AddModelError("NError", "User name length should be more than 3 characters.");
                }
            }

            if (customer.Password == null)
            {
                ModelState.AddModelError("PError", "Password is required.");
            }
            else
            {
                if (customer.Password.Length < 5)
                {
                    ModelState.AddModelError("PError", "Password length should be more than 5 characters.");
                }
            }

            
            if (customer.Address== null)
            {
                ModelState.AddModelError("AError", "Delivery Address is required.");
            }

            if (customer.Address == null)
            {
                ModelState.AddModelError("PhError", "Delivery Address is required.");
            }



            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("LoggedIn", "yes");
                HttpContext.Session.SetString("CustomerName", customer.Name);

                _context.Add(customer);
                await _context.SaveChangesAsync();

                var cid= await _context.Customers.FirstOrDefaultAsync(c=>c.Name==customer.Name);
                HttpContext.Session.SetInt32("CustomerId", cid.CustomerId);

                return RedirectToAction("Index", "Products");
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
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

            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

           

          
            if (customer.Name == null)
            {
                ModelState.AddModelError("NError", "User name is required");
            }
            else
            {
                if (customer.Name.Length < 3)
                {
                    ModelState.AddModelError("NError", "User name length should be more than 3 characters.");
                }
            }

            if (customer.Password == null)
            {
                ModelState.AddModelError("PError", "Password is required.");
            }
            else
            {
                if (customer.Password.Length < 5)
                {
                    ModelState.AddModelError("PError", "Password length should be more than 5 characters.");
                }
            }


            if (customer.Address == null)
            {
                ModelState.AddModelError("AError", "Delivery Address is required");
            }

            if (customer.Address == null)
            {
                ModelState.AddModelError("AError", "Phone Number is required");
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            return View(customer);
        }

        

        // POST: Customers/Delete/5
        
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'FinalProjectContext.Customers'  is null.");
            }
            

            var customer = await _context.Customers.FindAsync(id);
            var inv = await _context.Invoices.ToListAsync();

            foreach (var cus in inv) 
            {
                if (cus.CustomerId == id)
                {
                    _context.Invoices.Remove(cus);
                }
            }


            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }




        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
