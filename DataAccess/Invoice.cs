﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TVShop.DataAccess
{
    public partial class Invoice
    {
        public string CustomerId { get; set; } = null!;
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Television Product { get; set; } = null!;
    }
}
