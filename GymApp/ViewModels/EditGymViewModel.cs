using GymApp.Data.Enum;
using GymApp.Models;

namespace GymApp.ViewModels
{
    public class EditGymViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public GymCategory GymCategory { get; set; }
    }
}
