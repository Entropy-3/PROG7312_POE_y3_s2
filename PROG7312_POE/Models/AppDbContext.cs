using Microsoft.EntityFrameworkCore;

namespace PROG7312_POE.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<userTBL> Users { get; set; }
        public DbSet<issueTBL> Issues { get; set; }
    }
}
