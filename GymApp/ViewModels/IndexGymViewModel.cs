using GymApp.Models;

namespace GymApp.ViewModels;

public class IndexGymViewModel
{
    public IEnumerable<Gym> Gyms { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalGyms { get; set; }
    public int PageSize { get; set; }
    public int Category { get; set; }
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}