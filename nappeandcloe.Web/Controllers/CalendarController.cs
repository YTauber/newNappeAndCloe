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
            return calendarEvents;
        }
    }
}