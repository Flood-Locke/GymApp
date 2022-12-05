using GymApp.Data;
using GymApp.Data.Enum;
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
        public async Task<Gym?> GetByIdAsync(int id)
        {
            return await _context.Gyms.Include(id => id.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Gym?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Gyms.Include(id => id.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<Gym>> GetSliceAsync(int offset, int size)
        {
            return await _context.Gyms.Include(i => i.Address).Skip(offset).Take(size).ToListAsync();
        }
        public async Task<IEnumerable<Gym>> GetGymsByCategoryAndSliceAsync(GymCategory category, int offset, int size)
        {
            return await _context.Gyms
                .Include(i => i.Address)
                .Where(g => g.GymCategory == category)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
        }
        public async Task<int> GetCountAsync()
        {
            return await _context.Gyms.CountAsync();
        }
        public async Task<int> GetCountByCategoryAsync(GymCategory category)
        {
            return await _context.Gyms.CountAsync(c => c.GymCategory == category);
        }
        public async Task<List<Province>> GetAllProvinces()
        {
            return await _context.Provinces.ToListAsync();
        }
        public async Task<IEnumerable<Gym>> GetGymsByProvince(string province)
        {
            return await _context.Gyms.Where(g => g.Address.Province.Contains(province)).ToListAsync();
        }
        public async Task<List<City>> GetAllCitiesByProvince(string province)
        {
            return await _context.Cities.Where(c => c.ProvinceCode.Contains(province)).ToListAsync();
        }

        //Goes into Gym then into Address Then search by City
        public async Task<IEnumerable<Gym>> GetGymByCity(string city)
        {
            return await _context.Gyms.Where(g => g.Address.City.Contains(city)).Distinct().ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Gym gym)
        {
            _context.Update(gym);
            return Save();
        }
    }
}
