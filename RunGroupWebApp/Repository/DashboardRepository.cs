﻿using Microsoft.EntityFrameworkCore;
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
            var curUser = _httpContext.HttpContext?.User.GetUserId();
           // var userId = curUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == curUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = _httpContext.HttpContext?.User.GetUserId();
           // var userId = curUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRaces = _context.Races.Where(r => r.AppUser.Id == curUser);
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync(); // FirstOrDefaultAsync only return 1 user
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
