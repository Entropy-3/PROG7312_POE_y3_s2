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
        //displays all events
        [HttpGet]
        public IActionResult Events()
        {
            var allEvents = _eventService.GetAllEvents();
            return View(allEvents);
        }

        [HttpGet]
        public IActionResult UpcomingEvents()
        {
            var queue = _eventService.GetUpcomingEventsQueue(DateTime.Now);
            return View(queue);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //displays the AddEvent view
        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEvent(eventTBL model)
        {
            if (ModelState.IsValid)
            {
                _eventService.AddEvent(model);
                TempData["SuccessMessage"] = $"Event '{model.EventName}' added successfully!";
                return RedirectToAction("Events");
            }

            // If validation fails, re-display form with errors
            return View(model);
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\