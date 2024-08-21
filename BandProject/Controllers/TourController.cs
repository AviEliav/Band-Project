//using AspNetCore;
using BandProject.Data;
using BandProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BandProject.Controllers
{
    public class TourController : Controller
    {
        private readonly BandProjectContext _context;
        List<Tour> tour = new List<Tour>();

        public TourController(BandProjectContext context)
        {
            _context = context;
            tour = _context.Tours.ToList();
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("events") == null)
            {
                var events = tour.Select(e => new EventItem { Id = e.Id, Arena = e.Arena, Bought = false }).ToList();
                var json = JsonConvert.SerializeObject(events);
                HttpContext.Session.SetString("events", json);
            }
            var eventObject = JsonConvert.DeserializeObject<List<EventItem>>(HttpContext.Session.GetString("events"));
            ViewBag.Events = eventObject;

            return View(await _context.Tours.ToListAsync());
        }

        [HttpPost]
        public IActionResult TicketBought(int id)
        {
            var json = HttpContext.Session.GetString("events");
            // Explicitly specify the type to List<dynamic>
            var events = JsonConvert.DeserializeObject<List<EventItem>>(json);
            var eventItem = events.FirstOrDefault(e => e.Id == id);
            if (eventItem != null)
            {
                eventItem.Bought = true;
                var jsonObject = JsonConvert.SerializeObject(events);
                HttpContext.Session.SetString("events", jsonObject); // Update with new JSON
            }
            return RedirectToAction("Index");
        }
    }
}
