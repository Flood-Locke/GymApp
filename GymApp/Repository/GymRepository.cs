using GymApp.Data;
using GymApp.Interfaces;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repository
{
    public class GymRepository : IGymRepository
    {
        private readonly ApplicationDbContext _context;

        public GymRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Gym gym)
        {
            _context.Add(gym);
            return Save();
        }

        public bool Delete(Gym gym)
        {
            _context.Remove(gym);
            return Save();
        }
        //Return Full List
        public async Task<IEnumerable<Gym>> GetAll()
        {
            return await _context.Gyms.ToListAsync();
        }
        //Return one instance
        public async Task<Gym> GetByIdAsync(int id)
        {
            return await _context.Gyms.Include(id => id.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Gym> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Gyms.Include(id => id.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        //Goes into Gym then into Address Then search by City
        public async Task<IEnumerable<Gym>> GetGymByCity(string city)
        {
            return await _context.Gyms.Where(g => g.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Gym gym)
        {
            _context.Update(gym);
            return Save();
        }
    }
}
