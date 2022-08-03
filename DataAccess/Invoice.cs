using System;
using System.Collections.Generic;

namespace TVShop.DataAccess
{
    public partial class Invoice
    {
        public string? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? Date { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Television? Product { get; set; }
    }
}
