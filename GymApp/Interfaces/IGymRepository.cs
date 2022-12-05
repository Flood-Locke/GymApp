using GymApp.Models;
using GymApp.Data.Enum;

namespace GymApp.Interfaces
{
    public interface IGymRepository
    {
        Task<IEnumerable<Gym>> GetAll();
        Task<IEnumerable<Gym>> GetSliceAsync(int offset, int size);
        Task<IEnumerable<Gym>> GetGymByCity(string city);
        Task<IEnumerable<Gym>> GetGymsByProvince(string province);
        Task<IEnumerable<Gym>> GetGymsByCategoryAndSliceAsync(GymCategory category, int offset, int size);
        Task<Gym?> GetByIdAsync(int id);
        Task<Gym> GetByIdAsyncNoTracking(int id);
        Task<List<Province>> GetAllProvinces();
        Task<List<City>> GetAllCitiesByProvince(string province);
        Task<int> GetCountAsync();
        Task<int> GetCountByCategoryAsync(GymCategory category);     
        bool Add(Gym gym);
        bool Update(Gym gym);
        bool Delete(Gym gym);
        bool Save();
    }
}
