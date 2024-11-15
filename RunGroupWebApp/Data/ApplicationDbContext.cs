using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data
{
    public class ApplicationDbContext: DbContext // inherits from EF
    {
        // 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        public DbSet<Race> Races { get; set; } 
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Address> Addresses { get; set; }
        public object Race { get; internal set; }
    }
}
