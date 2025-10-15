using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Services.Implementation
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }


        public void AddEvent(eventTBL newEvent)
        {
            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }

        public List<eventTBL> GetAllEvents()
        {
            return _context.Events
                      .OrderBy(e => e.EventDate)
                      .ToList();
        }

        public Dictionary<string, List<eventTBL>> GetEventsGroupedByCategory()
        {
            throw new NotImplementedException();
        }

        public HashSet<string> GetUniqueCategories()
        {
            throw new NotImplementedException();
        }

        // Queue of upcoming events (computed from DB)
        public Queue<eventTBL> GetUpcomingEventsQueue(DateTime? from = null)
        {
            var start = from ?? DateTime.UtcNow;

            var ordered = _context.Events
                             .Where(e => e.EventDate >= start)
                             .OrderBy(e => e.EventDate)
                             .ToList();

            return new Queue<eventTBL>(ordered);
        }
    }
}
