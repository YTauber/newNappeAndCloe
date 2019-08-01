using System;
using System.Collections.Generic;
using System.Text;

namespace nappeandcloe.Data
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public ProductSize ProductSize { get; set; }
        public int ProductSizeId { get; set; }

        public Order Order { get; set; }
        public int OrderId { get; set; }

        public int Quantity { get; set; }
        public decimal PricePer { get; set; }

        public int PickUps { get; set; }
        public int Returns { get; set; }
        public int ReturnedNotUsed { get; set; }
        public int Damages { get; set; }
        public int Losts { get; set; }
    }

    //public class PickUp
    //{
    //    public int Id { get; set; }

    //    public OrderDetail OrderDetail { get; set; }
    //    public int OrderDetailId { get; set; }

    //    public int Quantity { get; set; }
    //    public DateTime Date { get; set; }
    //}

    //public class Return
    //{
    //    public int Id { get; set; }

    //    public OrderDetail OrderDetail { get; set; }
    //    public int OrderDetailId { get; set; }

    //    public int Quantity { get; set; }
    //    public DateTime Date { get; set; }
    //    public int UsedAmount { get; set; }
    //}

    //public class Damage
    //{
    //    public int Id { get; set; }

    //    public OrderDetail OrderDetail { get; set; }
    //    public int OrderDetailId { get; set; }

    //    public int Quantity { get; set; }
    //    public DateTime Date { get; set; }
    //    public decimal Charge { get; set; }
    //    public int FixedAmount { get; set; }
    //}

    //public class Lost
    //{
    //    public int Id { get; set; }

    //    public OrderDetail OrderDetail { get; set; }
    //    public int OrderDetailId { get; set; }

    //    public int Quantity { get; set; }
    //    public DateTime Date { get; set; }
    //    public decimal Charge { get; set; }
    //    public int LostAmount { get; set; }
    //}
}
