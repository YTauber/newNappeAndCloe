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
        public void AddOrderDetailToDraft(ProductView product)
        {
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();

            ProductView productView =  order.ProductViews.FirstOrDefault(p => p.Id == product.Id);
            order.ProductViews.Remove(productView);
            order.ProductViews.Add(product);

            order = SetTotal(order);

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

            order.TaxExemt = c.TaxExemt;
            order.Address = c.Address;

            order = SetTotal(order);
            HttpContext.Session.Set("order", order);
        }

        [Route("AddDateDraft")]
        [HttpPost]
        public void AddDateDraft(OrderView orderView)
        {
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            order.Date = orderView.Date;
            
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


        private OrderView SetAvail(OrderView order)
        {
            ProductRepository productRepo = new ProductRepository(_connectionString);

            foreach (ProductSizeView size in order.ProductViews.SelectMany(p => p.ProductSizeViews))
            {
                size.MaxAvail = order.Date == null ? 0 : productRepo.GetMaxAvail(order.Date.Value, size.Id);
                if (size.OrderAmount > size.MaxAvail)
                {
                    size.OrderAmount = size.MaxAvail;
                }
            }

            return order;
        }

        private OrderView SetTotal(OrderView order)
        {
            double t = 1.08735;
            decimal ta = (decimal)t;
            order.Total = 0;
            order.Tax = 0;
            order.DiscuntAmount = 0;
            foreach (ProductSizeView size in order.ProductViews.SelectMany(p => p.ProductSizeViews))
            {
                order.Total += size.OrderAmount * size.PricePer;
            }
            order.Total += order.Liner.Quantity * order.Liner.Cahrge;
            order.Total += order.DeliveryCharge;
            order.DiscuntAmount = (order.Total * order.Discount) / 100;
            if (!order.TaxExemt)
            {
                order.Tax = (order.Total * ta) - order.Total;
                order.Total += order.Tax;
            }
            order.Total -= order.DiscuntAmount;
            return order;
        }

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