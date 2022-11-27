using GymApp.Models;
namespace GymApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<Gym> Gyms { get; set; }
        public List<WorkoutProgram> WorkoutPrograms { get; set; }
    }
}