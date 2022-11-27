using GymApp.Data;
using GymApp.Interfaces;
using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repository
{
    public class WorkoutProgramRepository : IWorkoutProgramRepository
    { 
        private readonly ApplicationDbContext _context;

        public WorkoutProgramRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(WorkoutProgram workoutProgram)
        {
            _context.Add(workoutProgram);
            return Save();
        }

        public bool Delete(WorkoutProgram workoutProgram)
        {
            _context.Remove(workoutProgram);
            return Save();
        }

        public async Task<IEnumerable<WorkoutProgram>> GetAll()
        {
            return await _context.WorkoutPrograms.ToListAsync();
        }

        public async Task<WorkoutProgram> GetByIdAsync(int id)
        {
            return await _context.WorkoutPrograms.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<WorkoutProgram> GetByIdAsyncNoTracking(int id)
        {
            return await _context.WorkoutPrograms.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(WorkoutProgram workoutProgram)
        {
            _context.Update(workoutProgram);
            return Save();
        }
    }
}
