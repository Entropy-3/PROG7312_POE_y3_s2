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
        public async Task<IActionResult> Events()
        {
            var allEvents = await _eventService.GetAllEventsAsync();
            return View(allEvents);
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
        //displays the AddEvent view
        [HttpGet]
        public IActionResult AddEvent()
        {
            return View();
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