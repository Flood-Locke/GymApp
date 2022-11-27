using GymApp.Models;

namespace GymApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Gym>> GetAllUserGyms();
        Task<List<WorkoutProgram>> GetAllUserWorkouts();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}