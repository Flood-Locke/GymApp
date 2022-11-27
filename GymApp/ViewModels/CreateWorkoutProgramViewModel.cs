using GymApp.Data.Enum;

namespace GymApp.ViewModels
{
    public class CreateWorkoutProgramViewModel
    {     
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public WorkoutProgramCategory WorkoutProgramCategory { get; set; }
        public string? AppUserId { get; set; }
        
    }
}
