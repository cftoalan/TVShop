using System;
using System.Collections.Generic;

namespace TVShop.DataAccess
{
    public partial class Television
    {
        public Television()
        {
            Invoices = new HashSet<Invoice>();
        }

        public int ProductId { get; set; }
        public string? Model { get; set; }
        public int? ManufacturerId { get; set; }
        public string? Resolution { get; set; }
        public string? HdrSupport { get; set; }
        public string? ScreenSize { get; set; }
        public double? Price { get; set; }
        public int? Inventory { get; set; }

        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
