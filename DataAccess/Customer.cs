using System;
using System.Collections.Generic;

namespace TVShop.DataAccess
{
    public partial class Customer
    {
        public Customer()
        {
            Invoices = new HashSet<Invoice>();
        }

        public string CustomerId { get; set; } = null!;
        public string? Name { get; set; }
        public string? Password { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
