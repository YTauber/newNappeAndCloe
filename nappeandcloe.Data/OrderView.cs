﻿using System;
using System.Collections.Generic;
using System.Text;

namespace nappeandcloe.Data
{
    public class OrderView
    {
        public OrderView()
        {
            ProductViews = new List<ProductView>();
            Liner = new Liner();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime? Date { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Discount { get; set; }
        public string Notes { get; set; }
        public Liner Liner { get; set; }

        public decimal Total { get; set; }
        public bool TaxExemt { get; set; }
        public decimal Tax { get; set; }
        public decimal DiscuntAmount { get; set; }


        public List<ProductView> ProductViews { get; set; }

        public List<Payment> Payments { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }

    public class Inventory
    {
        public Inventory()
        {
            ProductViews = new List<ProductView>();
            Sizes = new List<Size>();
            Labels = new List<Label>();
        }
        public List<ProductView> ProductViews { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Label> Labels { get; set; }
    }

    public class ProductView
    {
        public ProductView()
        {
            ProductSizeViews = new List<ProductSizeView>();
            CalendarEvents = new List<CalendarEvent>();
            ProductLabels = new List<ProductLabel>();
            ProductSizes = new List<ProductSize>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureName { get; set; }
        public string Notes { get; set; }

        public List<ProductSize> ProductSizes { get; set; }
        public List<ProductLabel> ProductLabels { get; set; }

        public List<ProductSizeView> ProductSizeViews { get; set; }

        public List<CalendarEvent> CalendarEvents { get; set; }
    }

    public class ProductSizeView
    {
        public int Id { get; set; }
        public string Size { get; set; }

        public int Quantity { get; set; }
        public decimal PricePer { get; set; }

        public int MinAvail { get; set; }
        public int MaxAvail { get; set; }

        public bool Checked { get; set; }

        public decimal OrderPrice { get; set; }
        public int OrderAmount { get; set; }
    }



    public class DayView
    {
        public DayView()
        {
            CalendarEvents = new List<CalendarEvent>();
            Orders = new List<OrderView>();
        }
        public List<CalendarEvent> CalendarEvents { get; set; }
        public List<OrderView> Orders { get; set; }
    }


}
