using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Implementation
{
    public class EventService
    {
        // Queue
        private Queue<eventTBL> eventQueue = new Queue<eventTBL>();

        // Dictionary that store events by category
        private Dictionary<string, List<eventTBL>> categorizedEvents = new Dictionary<string, List<eventTBL>>();

        // Set that unique list of event categories
        private HashSet<string> uniqueCategories = new HashSet<string>();

        public void AddEvent(eventTBL newEvent)
        {
            // Add to queue
            eventQueue.Enqueue(newEvent);

            // Add to set
            uniqueCategories.Add(newEvent.EventCategory);

            // Add to dictionary
            if (!categorizedEvents.ContainsKey(newEvent.EventCategory))
            {
                categorizedEvents[newEvent.EventCategory] = new List<eventTBL>();
            }

            categorizedEvents[newEvent.EventCategory].Add(newEvent);
        }

        public List<eventTBL> GetAllEvents()
        {
            // Combine all events from the dictionary
            List<eventTBL> allEvents = new List<eventTBL>();

            foreach (var category in categorizedEvents.Values)
            {
                allEvents.AddRange(category);
            }

            return allEvents;
        }

        public HashSet<string> GetUniqueCategories()
        {
            return uniqueCategories;
        }
    }
}
