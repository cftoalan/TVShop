using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TVShop.DataAccess
{
    public partial class Invoice
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        public int Quantity { get; set; }


        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Television Product { get; set; } = null!;
    }
}
