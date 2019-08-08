using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nappeandcloe.Data
{
    public class OrderRepository
    {
        private string _connectionString;
        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Liner AddLiner(Liner liner)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                context.Liners.Add(liner);
                context.SaveChanges();
                return liner;
            }
        }

        public int AddOrder(Order order)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                context.Orders.Add(order);
                context.SaveChanges();
                return order.Id;
            }
        }

        public void AddOrderDetails(IEnumerable<OrderDetail> orderDetails)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                context.OrderDetails.AddRange(orderDetails);
                context.SaveChanges();
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Orders.Include(o => o.OrderDetails).ToList();
            }
        }

        public IEnumerable<CalendarEvent> GetOrdersForCalendar(int month, int year)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Orders.Include(o => o.Customer).Where(o => o.Date.Month == month && o.Date.Year == year).ToList().Select(o =>
                {
                    return new CalendarEvent
                    {
                        Id = o.Id,
                        From = o.Date,
                        To = o.Date,
                        title = $"{o.Name} for {o.Customer.Name}",
                        Color= "#fd3153"
                    };
                });
            }
        }

        public IEnumerable<CalendarEvent> GetOrdersForCalendarByProductId(int month, int year, int productId)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Orders.Include(o => o.Customer).Where(o => o.Date.Month == month && o.Date.Year == year && o.OrderDetails.Any(od => od.ProductSize.ProductId == productId)).ToList().Select(o =>
                {
                    return new CalendarEvent
                    {
                        Id = o.Id,
                        From = o.Date,
                        To = o.Date,
                        title = $"{o.Name} for {o.Customer.Name}",
                        Color = "#fd3153"
                    };
                });
            }
        }

        public IEnumerable<Order> GetOrdersByDate(DateTime date)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Orders.Include(o => o.OrderDetails).Where(o => o.Date == date).ToList();
            }
        }
    }
}
