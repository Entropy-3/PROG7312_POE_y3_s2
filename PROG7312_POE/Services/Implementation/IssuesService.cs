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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //Add issue
        //chat gpt assisted me with the logic for saving an attatchment to the database
        public async Task<issueTBL?> AddIssueAsync(issueTBL issue, IFormFile attachment)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while adding issue: {ex.Message}");
                return null;
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\
        //returns issues from database 
        public async Task<List<issueTBL>> GetAllIssuesAsync()
        {
            try
            {
                return await _context.Issues.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while retrieving issues: {ex.Message}");
                //chat helped with return statement in order to avoid null reference exceptions by returning a null list
                return new List<issueTBL>();
            }
        }
    }
}
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~EOF~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\\