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
        //Events
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\

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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //returns a hashset of unique event categories
        public async Task<HashSet<string>> GetUniqueCategoriesAsync()
        {
            var categories = await _context.Events.Select(e => e.EventCategory).Distinct().ToListAsync();

            return new HashSet<string>(categories);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //groups events by date using DateOnly
        public async Task<Dictionary<DateOnly, List<eventTBL>>> GetEventsByDateAsync()
        {
            var groups = await _context.Events.GroupBy(e => DateOnly.FromDateTime(e.EventDate.Date))
                .Select(g => new
                {
                    //key is the date, items are the events on that date ordered by event date
                    Key = g.Key,
                    Items = g.OrderBy(x => x.EventDate).ToList()
                })
                .ToListAsync();

            return groups.ToDictionary(x => x.Key, x => x.Items);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //filters events by category and/or date
        //chat gpt assisted me with this method
        public async Task<List<eventTBL>> GetByCategoryAndDateAsync(string? category, DateOnly? date)
        {
            //builds the initial query
            var q = _context.Events.AsQueryable();

            //queries the collection based on the user chosen category, can be null
            if (!string.IsNullOrWhiteSpace(category))
                q = q.Where(e => e.EventCategory == category);

            //queries the collection based on the user chosen date, can be null
            if (date.HasValue)
            {
                var d = date.Value;
                q = q.Where(e => DateOnly.FromDateTime(e.EventDate.Date) == d);
            }

            //returns list of searched category and/or date ordered by event date ordered by event date
            return await q.OrderBy(e => e.EventDate).ToListAsync();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //queue of upcoming events
        public async Task<Queue<eventTBL>> GetUpcomingEventsQueueAsync(DateTime? from = null)
        {
            var start = from ?? DateTime.UtcNow;

            //chatgpt assisted me with the event date filtering logic
            var ordered = await _context.Events.Where(e => e.EventDate >= start).OrderBy(e => e.EventDate).ToListAsync();

            //returns the ordered list as a queue
            return new Queue<eventTBL>(ordered);
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //Announcements
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\

        //method that adds a new announcement to the database
        public async Task AddAnnouncementAsync(announcementTBL newAnnouncement)
        {
            await _context.Announcements.AddAsync(newAnnouncement);
            await _context.SaveChangesAsync();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //returns all announcements from the database
        public async Task<List<announcementTBL>> GetAllAnnouncementsAsync()
        {
            //returns all announcements from the database
            return await _context.Announcements.ToListAsync();
        }

    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\