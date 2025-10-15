using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IEventService
    {
        Task AddEventAsync(eventTBL newEvent);
        Task<List<eventTBL>> GetAllEventsAsync();
        Task<Queue<eventTBL>> GetUpcomingEventsQueueAsync(DateTime? from = null);
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\