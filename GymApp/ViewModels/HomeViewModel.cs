using GymApp.Models;

namespace GymApp.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Gym> Gyms { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }
}
