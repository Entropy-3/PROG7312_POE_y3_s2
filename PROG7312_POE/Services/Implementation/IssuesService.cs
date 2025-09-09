using Microsoft.EntityFrameworkCore;
using PROG7312_POE.Models;
using PROG7312_POE.Services.Interface;

namespace PROG7312_POE.Services.Implementation
{
    public class IssuesService : IIssuesService
    {
        private readonly AppDbContext _context;

        public IssuesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<issueTBL> AddIssueAsync(issueTBL issue, IFormFile attachment)
        {
            if (attachment != null && attachment.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await attachment.CopyToAsync(memoryStream);
                issue.DocumentData = memoryStream.ToArray();
            }

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issue;
        }
        public async Task<List<issueTBL>> GetAllIssuesAsync()
        {
            return await _context.Issues.ToListAsync();
        }
    }
}
