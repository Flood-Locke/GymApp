using Microsoft.EntityFrameworkCore;
using GymApp.Data;
using GymApp.Interfaces;
using GymApp.Models;
using GymApp;

namespace RunGroopWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Gym>> GetAllUserGyms()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userGymss = _context.Gyms.Where(r => r.AppUser.Id == curUser);
            return userGymss.ToList();
        }

        public async Task<List<WorkoutProgram>> GetAllUserWorkouts()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userWorkouts = _context.WorkoutPrograms.Where(r => r.AppUser.Id == curUser);
            return userWorkouts.ToList();
        }
        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
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