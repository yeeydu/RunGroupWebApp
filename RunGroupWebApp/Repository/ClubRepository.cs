using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context) // constructor to initialize database
        {
            _context = context;
        }

        public bool Add(Club club)
        {
          
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
           _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll() //
        {
            return await _context.Clubs.ToListAsync(); // async ? ToListAsync else ToList  // ToList = exe SQL and returns a list
        }

        public async Task<Club> GetById(int id)
        {
            // Include explicitly tell for (navigation property) on entity or (one to many relationship) bring the whole identity
            return await _context.Clubs.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id); // async ? FirstOrDefaultAsync else FirstOrDefault
        }


        public async Task<Club> GetByIdNoTracking(int id)
        {
             
            return await _context.Clubs.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);  
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
           var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _context.Update(club);  
            return Save();
        }
    }
}
