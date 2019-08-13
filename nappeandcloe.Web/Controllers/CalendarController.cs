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
    public class CalendarController : ControllerBase
    {
        private string _connectionString;
        public CalendarController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Route("GetCalendarEvents/{month}/{year}")]
        [HttpGet]
        public IEnumerable<CalendarEvent> GetCalendarEvents(int month, int year)
        {
            OrderRepository orderRepo = new OrderRepository(_connectionString);
            HebCalRepository hebCalrepo = new HebCalRepository();

            List<CalendarEvent> calendarEvents =  hebCalrepo.GetJewishEvents(month, year).ToList();
            calendarEvents.AddRange(orderRepo.GetOrdersForCalendar(month, year));

            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            if (order.Date.HasValue && order.Date.Value.Year == year && order.Date.Value.Month == month)
            {
                calendarEvents.Add(new CalendarEvent
                {
                    title = "Draft",
                    Id = 12212,
                    From = order.Date.Value,
                    To = order.Date.Value,
                    Color = "#125422"
                });
            }

            return calendarEvents;
        }

        [Route("GetDay/{month}/{year}/{day}")]
        [HttpGet]
        public DayView GetDay(int month, int year, int day)
        {
            HebCalRepository hebCalrepo = new HebCalRepository();
            OrderRepository orderRepo = new OrderRepository(_connectionString);
            OrderView order = HttpContext.Session.Get<OrderView>("order") ?? new OrderView();
            OrderViewRepository viewRepo = new OrderViewRepository(_connectionString, order);

            DayView dayView = new DayView();

            dayView.CalendarEvents = hebCalrepo.GetJewishEvents(month, year).Where(c => c.From.Day == day).ToList();

            if (order.Date.HasValue && order.Date.Value.Year == year && order.Date.Value.Month == month && order.Date.Value.Day == day)
            {
                dayView.CalendarEvents.Add(new CalendarEvent
                {
                    title = "Draft",
                    Id = 12212,
                    From = order.Date.Value,
                    To = order.Date.Value,
                    Color = "#125422"
                });
            }

            List<Order> orders = orderRepo.GetOrdersByDate(new DateTime(year, month, day)).ToList();

            dayView.Orders = viewRepo.GetOrderViewsForOrders(orders).ToList();

            return dayView;
        }

        
    }
}