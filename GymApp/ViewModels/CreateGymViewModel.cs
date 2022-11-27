using GymApp.Data.Enum;
using GymApp.Models;

namespace GymApp.ViewModels
{
    public class CreateGymViewModel
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public GymCategory GymCategory { get; set; }
        public string AppUserId { get; set; }
    }
}
