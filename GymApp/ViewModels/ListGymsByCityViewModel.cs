using GymApp.Models;

namespace GymApp.ViewModels
{
    public class ListGymsByCityViewModel
    {
        public IEnumerable<Gym> Gyms { get; set; }
        public bool NoGymWarning { get; set; } = false;
        public string City { get; set; }
        public string Province { get; set; }
    }
}
