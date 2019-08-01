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

        [HttpPost]
        [Route("AddOrder")]
        public int AddOrder(Order order) 
        {
            OrderRepository OrderRepo = new OrderRepository(_connectionString);
            
            OrderRepo.AddOrder(order);
           
            return order.Id;
        }

        [HttpPost]
        [Route("AddOrderDetails")]
        public void AddOrderDetails(OrderView2 order)
        {
            OrderRepository OrderRepo = new OrderRepository(_connectionString);
            foreach (OrderDetail od in order.OrderDetails)
            {
                od.OrderId = order.OrderId;
                od.ProductSize = null;
            }
            OrderRepo.AddOrderDetails(order.OrderDetails);
        }
    }

    public class OrderView2
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}