using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IEventService
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //Events
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\

        Task AddEventAsync(eventTBL newEvent);

        Task<List<eventTBL>> GetAllEventsAsync();

        Task<HashSet<string>> GetUniqueCategoriesAsync();
        Task<Dictionary<DateOnly, List<eventTBL>>> GetEventsByDateAsync();

        Task<List<eventTBL>> GetByCategoryAndDateAsync(string? category, DateOnly? date);
        Task<Queue<eventTBL>> GetUpcomingEventsQueueAsync(DateTime? from = null);

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //Announcements
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\

        Task AddAnnouncementAsync(announcementTBL newAnnouncement);
        Task<List<announcementTBL>> GetAllAnnouncementsAsync();
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\