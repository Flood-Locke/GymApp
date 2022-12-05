namespace GymApp.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public IFormFile Image { get; set; }
    }
}
