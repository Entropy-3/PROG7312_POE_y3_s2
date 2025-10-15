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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //uses a queue to display upcoming events
        [HttpGet]
        public IActionResult UpcomingEvents()
        {
            var queue = _eventService.GetUpcomingEventsQueue();
            return View(queue);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //displays the AddEvent view
        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that adds a new event to the database
        [HttpPost]
        public IActionResult AddEvent(eventTBL model)
        {
            if (ModelState.IsValid)
            {
                _eventService.AddEvent(model);
                //chatgpt assisted me with the model.EventName syntax
                TempData["SuccessMessage"] = $"Event '{model.EventName}' added successfully!";
                return RedirectToAction("Events");
            }

            return View(model);
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\