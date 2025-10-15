using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IEventService
    {
        void AddEvent(eventTBL newEvent);
        List<eventTBL> GetAllEvents();
        HashSet<string> GetUniqueCategories();
    }
}
