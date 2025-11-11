using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IServiceService
    {
        Task<serviceTBL?> AddServiceAsync(serviceTBL service);
        Task<List<serviceTBL>> GetAllServicesAsync();

        // Handy extras for your tracking page / actions:
        Task<serviceTBL?> GetByIdAsync(int id);
        Task<bool> AdvanceStatusAsync(int id, RequestStatus next);
    }
}
