using GymApp.Models;

namespace GymApp.ViewModels;

public class IndexGymViewModel
{
    public IEnumerable<Gym> Gyms { get; set; }
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalGyms { get; set; }
    public int PageSize { get; set; }

    public IndexGymViewModel(IEnumerable<Gym> gyms, int pageIndex, int pageSize)
    {
        Gyms = gyms;
        PageIndex = pageIndex;
        TotalGyms = gyms.Count();
        TotalPages = (int)Math.Ceiling(TotalGyms / (double)pageSize);
        PageSize = pageSize;
        Gyms = gyms.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}