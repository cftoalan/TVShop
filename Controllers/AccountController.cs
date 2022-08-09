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
    public class AccountController : Controller
    {
        private readonly FinalProjectContext _context;

        public AccountController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Login()
        {
            return View();
                          
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Customer customer)
        {
            var customerContext = await _context.Customers.ToListAsync();

            bool idFound = false;
            bool pwFound = false;
            string name = "";
            foreach (Customer cust in customerContext)
            {
                if (cust.CustomerId == customer.CustomerId)
                {
                    name = cust.Name;
                    idFound = true;

                    if (cust.Password == customer.Password)
                    {
                        pwFound = true;
                    }
                }
            }

            if (customer.CustomerId==null)
            {
                ModelState.AddModelError("IdError", "Please enter the User ID");
            }
            if (customer.Password==null)
            {
                ModelState.AddModelError("PasswordError", "Please enter the Password");
            }

            if (!idFound)
            {
                ModelState.AddModelError("IdError", "ID not found");
            }
            else
            {
                if (!pwFound)
                {
                    ModelState.AddModelError("PasswordError", "Incorrect Password");
                }
            }

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("LoggedIn", "yes");
                HttpContext.Session.SetString("CustomerId", customer.CustomerId.ToString());
                HttpContext.Session.SetString("CustomerName", name);
                return RedirectToAction("AccountDetail", new { id = customer.CustomerId });
            }
            return View(customer);
        }


        public async Task<IActionResult> AccountDetail(string id)
        {
            if (HttpContext.Session.GetString("LoggedIn") == "yes")
            {
                ViewData["LoggedIn"] = "yes";
                id = HttpContext.Session.GetString("CustomerId");
            }
            else
            {
                ViewData["LoggedIn"] = "no";
            }
            var customerdt = await _context.Customers.Include(a => a.Invoices).FirstOrDefaultAsync(a => a.CustomerId == id);
            var invdb = await _context.Invoices.ToListAsync();
            var productdb = await _context.Televisions.ToListAsync();

            foreach (var item in customerdt.Invoices)

            {
                foreach (var invoice in invdb)
                {
                    item.CustomerId = invoice.CustomerId;
                }
                foreach (var television in productdb)
                {
                    item.Product = television;
                }

            }
            
            return View(customerdt);
        }



        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }








        private bool CustomerExists(string id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
