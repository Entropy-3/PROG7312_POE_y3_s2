using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IServiceService
    {
        Task<serviceTBL?> AddServiceAsync(serviceTBL service);
        Task<List<serviceTBL>> GetAllServicesAsync();
        Task<serviceTBL?> GetByIdAsync(int id);
        Task<bool> AdvanceStatusAsync(int id, RequestStatus next);
        Task<List<serviceTBL>> GetTopUrgentAsync(int count = 10);
        Task<List<serviceTBL>> GetRelatedAsync(int id);
    }
}
