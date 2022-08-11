using System;
using System.Collections.Generic;

namespace TVShop.DataAccess
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Televisions = new HashSet<Television>();
        }

        public int ManufacturerId { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Detail { get; set; }

        public virtual ICollection<Television> Televisions { get; set; }
    }
}
