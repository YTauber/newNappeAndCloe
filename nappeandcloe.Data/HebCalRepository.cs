using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace nappeandcloe.Data
{
    public class HebCalRepository
    {
        private IEnumerable<Event> GetEvents(int month, int year)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string Json = client.GetStringAsync($"https://www.hebcal.com/hebcal/?v=1&cfg=json&maj=on&min=on&year={year}&month={month}&mf=on&geo=geoname&geonameid=3448439&m=50&s=on").Result;
                    Events j = JsonConvert.DeserializeObject<Events>(Json);

                    return j.items;
                }
            }
            catch
            {
                return new List<Event>();
            }
            
        }

        public IEnumerable<CalendarEvent> GetJewishEvents(int month, int year)
        {
            IEnumerable<Event> e = GetEvents(month, year);
            List<CalendarEvent> jewishEvents = new List<CalendarEvent>();
            int i = 0;
            foreach (Event ev in e)
            {
                jewishEvents.Add(new CalendarEvent
                {
                    Id = i,
                    Color = "#1ccb9e",
                    From = ev.date,
                    To = ev.date,
                    title = ev.title
                });
                i++;
            }
            return jewishEvents;
        }

    }

    public class Event
    {
        public string title { get; set; }
        public DateTime date { get; set; }
    }
    public class Events
    {
        public Events()
        {
            items = new List<Event>();
        }
        public List<Event> items { get; set; }
    }

    public class CalendarEvent
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string title { get; set; }
    }
}
