using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Services.Implementation
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //Constructor that injects the database context
        public EventService(AppDbContext context)
        {
            _context = context;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //method that adds a new event to the database
        public void AddEvent(eventTBL newEvent)
        {
            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //returns all events from the database ordered by date
        public List<eventTBL> GetAllEvents()
        {
            //chatgpt assisted me with the OrderBy syntax
            return _context.Events.OrderBy(e => e.EventDate).ToList();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //queue of upcoming events
        public Queue<eventTBL> GetUpcomingEventsQueue(DateTime? from = null)
        {
            var start = from ?? DateTime.UtcNow;

            //chatgpt assisted me with the event date filtering logic
            var ordered = _context.Events.Where(e => e.EventDate >= start).OrderBy(e => e.EventDate).ToList();

            return new Queue<eventTBL>(ordered);
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\