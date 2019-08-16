using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using nappeandcloe.Data;

namespace nappeandcloe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private string _connectionString;
        public OrderController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("AddLiner")]
        public int AddLiner(Liner liner)
        {
            OrderRepository OrderRepo = new OrderRepository(_connectionString);
            OrderRepo.AddLiner(liner);
            return liner.Id;
        }

        [HttpGet]
        [Route("GetOrderById/{orderId}")]
        public OrderView GetOrderById(int orderId)
        {
            OrderRepository OrderRepo = new OrderRepository(_connectionString);

            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            OrderViewRepository viewRepo = new OrderViewRepository(_connectionString, order);

            Order o = OrderRepo.GetOrdersById(orderId);
            return viewRepo.GetOrderViewForOrder(o);
        }

        [HttpPost]
        [Route("AddOrder")]
        public void AddOrder() 
        {
            OrderRepository OrderRepo = new OrderRepository(_connectionString);
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();

            int orderId = OrderRepo.AddOrder(new Order
            {
                Address = order.Address,
                CustomerId = order.CustomerId,
                Date = order.Date.Value,
                Name = order.Name,
                Notes = order.Notes,
                TaxExemt = order.TaxExemt,
                DeliveryCharge = order.DeliveryCharge,
                Discount = order.Discount
            });
            OrderRepo.AddOrderDetails(order.ProductViews.SelectMany(p => p.ProductSizeViews).Select(s => new OrderDetail
            {
                OrderId = orderId,
                ProductSizeId = s.Id,
                Quantity = s.OrderAmount,
                PricePer = s.PricePer
            }));
        }
    }
}