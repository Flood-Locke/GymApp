using GymApp.Models;

namespace GymApp.ViewModels;

public class UserDetailViewModel
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public int? YearsOfExperience { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string ProfileImageUrl { get; set; }

    public string Location => (City, Province) switch
    {
        (string city, string province) => $"{city}, {province}",
        (string city, null) => city,
        (null, string province) => province,
        (null, null) => "",
    };
}
