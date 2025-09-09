using PROG7312_POE.Models;

namespace PROG7312_POE.Services.Interface
{
    public interface IIssuesService
    {
        Task<issueTBL> AddIssueAsync(issueTBL issue, IFormFile attachment);
        Task<List<issueTBL>> GetAllIssuesAsync();
    }
}
