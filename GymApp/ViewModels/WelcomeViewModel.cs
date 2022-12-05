namespace GymApp.ViewModels
{
    public class WelcomeViewModel
    {
        public int? YearsofExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public IFormFile? Image { get; set; }
    }
}
