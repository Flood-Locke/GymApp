using GymApp.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Models
{
    public class WorkoutProgram
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public WorkoutProgramCategory WorkoutProgramCategory { get; set; }
        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }      
        public AppUser? AppUser { get; set; }
    }
}
