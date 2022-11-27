using GymApp.Models;

namespace GymApp.Interfaces
{
    public interface IWorkoutProgramRepository
    {
        Task<IEnumerable<WorkoutProgram>> GetAll();
        Task<WorkoutProgram> GetByIdAsync(int id);
        Task<WorkoutProgram> GetByIdAsyncNoTracking(int id);

        bool Add(WorkoutProgram workoutProgram);
        bool Update(WorkoutProgram workoutProgram);
        bool Delete(WorkoutProgram workoutProgram);
        bool Save();
    }
}
