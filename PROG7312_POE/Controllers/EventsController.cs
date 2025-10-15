using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        // Constructor that injects the event service
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //displays the AddEvent view
        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //chatgpt assisted me with the viewbags
        [HttpGet]
        public async Task<IActionResult> Events(string? category, DateOnly? date)
        {
            var categoriesSet = await _eventService.GetUniqueCategoriesAsync();
            ViewBag.Categories = categoriesSet.OrderBy(c => c).ToList();

            var eventsByDate = await _eventService.GetEventsByDateAsync();
            ViewBag.Dates = eventsByDate.Keys.OrderBy(d => d).Select(d => d.ToString("yyyy-MM-dd")).ToList();

            //uses method in service to filter events by category and/or date
            var model = (category != null || date != null)
                ? await _eventService.GetByCategoryAndDateAsync(category, date)
                : await _eventService.GetAllEventsAsync();

            ViewBag.SelectedCategory = category ?? "";
            ViewBag.SelectedDate = date?.ToString("yyyy-MM-dd") ?? "";

            //returns the view with the model containing the filtered or all events
            return View(model);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //uses a queue to display upcoming events
        [HttpGet]
        public async Task<IActionResult> UpcomingEvents()
        {
            var queue = await _eventService.GetUpcomingEventsQueueAsync();
            return View(queue);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that adds a new event to the database
        [HttpPost]
        public async Task<IActionResult> AddEvent(eventTBL model)
        {
            //returns the view with validation errors if the model state is invalid
            if (!ModelState.IsValid)
                return View(model);

            await _eventService.AddEventAsync(model);
            //chat gpt assisted me with the model.EventName syntax
            TempData["SuccessMessage"] = $"Event '{model.EventName}' added successfully!";
            return RedirectToAction("Events");
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\