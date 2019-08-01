using System;
using System.Collections.Generic;
using System.Text;

namespace nappeandcloe.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool TaxExemt { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Discount { get; set; }
        public string Notes { get; set; }
        public int? LinerId { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<Payment> Payments { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public Order Order { get; set; }
        public int OrderId { get; set; }
    }

    public class Liner
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Cahrge { get; set; }
        public decimal MyCaharge { get; set; }

        public int PickUps { get; set; }
        public int Returns { get; set; }
        public int ReturnedNotUsed { get; set; }
        public int Damages { get; set; }
        public int Losts { get; set; }
    }
}
