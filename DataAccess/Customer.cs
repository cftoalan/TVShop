using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TVShop.DataAccess
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
        }


        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }


        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
