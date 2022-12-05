using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GymApp.Interfaces;
using GymApp.Models;
using GymApp.ViewModels;

namespace GymApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var userGyms = await _dashboardRespository.GetAllUserGyms();
            var userWorkouts = await _dashboardRespository.GetAllUserWorkouts();
            var dashboardViewModel = new DashboardViewModel()
            {
                Gyms = userGyms,
                WorkoutPrograms = userWorkouts
            };
            return View(dashboardViewModel);
        } 
    }
}
