using System;
using System.Collections.Generic;
using System.Text;

namespace nappeandcloe.Data
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool TaxExemt { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
