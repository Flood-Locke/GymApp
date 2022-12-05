using GymApp.Data.Enum;

namespace GymApp.ViewModels
{
    public class EditWorkoutProgramViewModel
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public WorkoutProgramCategory WorkoutProgramCategory { get; set; }


    }
}
