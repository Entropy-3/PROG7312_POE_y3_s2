using Microsoft.EntityFrameworkCore;
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
        public async Task AddEventAsync(eventTBL newEvent)
        {
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //returns all events from the database ordered by date
        public async Task<List<eventTBL>> GetAllEventsAsync()
        {
            //chatgpt assisted me with the OrderBy syntax
            return await _context.Events.OrderBy(e => e.EventDate).ToListAsync();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //queue of upcoming events
        public async Task<Queue<eventTBL>> GetUpcomingEventsQueueAsync(DateTime? from = null)
        {
            var start = from ?? DateTime.UtcNow;

            //chatgpt assisted me with the event date filtering logic
            var ordered = await _context.Events.Where(e => e.EventDate >= start).OrderBy(e => e.EventDate).ToListAsync();

            return new Queue<eventTBL>(ordered);
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\