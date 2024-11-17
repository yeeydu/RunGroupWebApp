using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using System.Security.Claims;

namespace RunGroupWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

       
        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) // giant object to access lot functionality 
        {
            _context = context;
            _httpContext = httpContext;
        }


        public async  Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContext.HttpContext?.User;
            var userId = curUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == userId);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContext.HttpContext?.User;
            var userId = curUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRaces = _context.Races.Where(r => r.AppUser.Id == userId);
            return userRaces.ToList();
        }

    }
}
