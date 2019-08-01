using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using nappeandcloe.Data;
using Newtonsoft.Json;

namespace nappeandcloe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftController : ControllerBase
    {
        private string _connectionString;
        public DraftController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Route("AddOrderDetailToDraft")]
        [HttpPost]
        public void AddOrderDetailToDraft(OrderView orderView)
        {
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            
            order = SetAvail(order);
            HttpContext.Session.Set("order", order);
        }

        [Route("AddCustomerDraft")]
        [HttpPost]
        public void AddCustomerDraft(Customer customer)
        {
            CostumerRepository costumerRepo = new CostumerRepository(_connectionString);
            Customer c = costumerRepo.GetCustomerById(customer.Id);

            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            order.CustomerId = customer.Id;
            order.Customer = new Customer { Id = c.Id, Name = c.Name };

            order.TaxExemt = c.TaxExemt;

            order = SetTotal(order);
            HttpContext.Session.Set("order", order);
        }

        [Route("AddDateDraft")]
        [HttpPost]
        public void AddDateDraft(OrderView orderView)
        {
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            order.Date = orderView.Date;
            order.OrderDetails.ForEach(d => d.Quantity = 0);
            order = SetAvail(order);
            order = SetTotal(order);
            HttpContext.Session.Set("order", order);
        }

        [Route("GetDraftOrder")]
        [HttpGet]
        public OrderView GetDraftOrder()
        {
            return HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
        }

        [Route("AddOrderDraft")]
        [HttpPost]
        public OrderView AddOrderDraft(OrderView order)
        {
            order = SetTotal(order);
            HttpContext.Session.Set("order", order);
            return order;
        }

        [Route("AddLinerDraft")]
        [HttpPost]
        public OrderView AddLinerDraft(Liner liner)
        {
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            order.Liner = new Liner { Quantity = liner.Quantity, Cahrge = liner.Cahrge, MyCaharge = liner.MyCaharge };
            if (order.Date != null)
            {
                order = SetTotal(order);
            }
            HttpContext.Session.Set("order", order);
            return order;
        }

        [Route("StartOver")]
        [HttpPost]
        public OrderView StartOver()
        {
            HttpContext.Session.Set("order", new OrderView());
            return new OrderView { Date = null };
        }


        private OrderView SetAvail(OrderView odv)
        {
            OrderRepository orderRepo = new OrderRepository(_connectionString);

            if (odv.Date != null)
            {
                IEnumerable<Order> orders = orderRepo.GetOrdersByDate(odv.Date.Value);
                foreach (OrderDetailsView d in odv.OrderDetails)
                {
                    d.MaxAvail = (d.ProductSize.Quantity) - (orders.SelectMany(o => o.OrderDetails.Where(od => od.ProductSizeId == d.ProductSizeId)).Sum(s => s.Quantity));
                }
            }
            else
            {
                odv.OrderDetails.ForEach(d => d.MaxAvail = 0);
            }
            
            return odv;
        }

        private OrderView SetTotal(OrderView odv)
        {
            double t = 1.08735;
            decimal ta = (decimal)t;
            odv.Total = 0;
            odv.Tax = 0;
            odv.DiscuntAmount = 0;
            foreach (OrderDetailsView d in odv.OrderDetails)
            {
                odv.Total += d.Quantity * d.PricePer;
            }
            odv.Total += odv.Liner.Quantity * odv.Liner.Cahrge;
            odv.Total += odv.DeliveryCharge;
            if (!odv.TaxExemt)
            {
                odv.Tax = (odv.Total * ta) - odv.Total;
                odv.Total += odv.Tax;
            }
            odv.DiscuntAmount = (odv.Total * odv.Discount) / 100;
            odv.Total -= odv.DiscuntAmount;
            return odv;
        }
     
    }


    public class OrderView
    {
        public OrderView()
        {
            OrderDetails = new List<OrderDetailsView>();
            Liner = new Liner();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal Discount { get; set; }
        public string Notes { get; set; }
        public Liner Liner { get; set; }

        public decimal Total { get; set; }
        public bool TaxExemt { get; set; }
        public decimal DiscuntAmount { get; set; }


        public List<OrderDetailsView> OrderDetails { get; set; }

        public List<Payment> Payments { get; set; }

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }

    public class OrderDetailsView
    {
        public ProductSize ProductSize { get; set; }
        public int ProductSizeId { get; set; }

        public Product Product { get; set; }


        public int Quantity { get; set; }
        public decimal PricePer { get; set; }

        public int MinAvail { get; set; }
        public int MaxAvail { get; set; }
    }



    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}