namespace GymApp.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string ProfileImageUrl { get; set; }

    //public string location => (city, province) switch
    //{
    //    (string city, string state) => $"{city}, {state}",
    //    (string city, null) => city,
    //    (null, string state) => state,
    //    (null, null) => "",
    //};
}