using GymApp.Models;

namespace GymApp.ViewModels
{
    public class ListGymsByProvinceViewModel
    {
        public IEnumerable<Gym> Gyms { get; set; }
        public bool NoGymWarning { get; set; } = false;
        public string Province { get; set; }
    }
}