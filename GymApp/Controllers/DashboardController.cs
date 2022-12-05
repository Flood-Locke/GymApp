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
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
            //_httpContextAccessor = httpContextAccessor;
        }
        //Create own mapper
        //private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        //{
        //    user.Id = editVM.Id;

        //    user.YearsOfExperience = editVM.YearsOfExperience;
        //    user.ProfileImageUrl = photoResult.Url.ToString();
        //    user.Address.City = editVM.City;
        //    user.Address.Province = editVM.Province;
        //}

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

        //[HttpGet]
        //public async Task<IActionResult> EditUserProfile()
        //{
        //    var curUserId = HttpContext.User.GetUserId();
        //    var user = await _dashboardRespository.GetUserById(curUserId);
            
        //    if (user == null) return View("Error");
            
        //    var editUserViewModel = new EditUserDashboardViewModel()
        //    {
        //        Id = curUserId,
        //        YearsOfExperience = user.YearsOfExperience,
        //        ProfileImageUrl = user.ProfileImageUrl,
        //        City = user.Address?.City,
        //        Province = user.Address?.Province,
        //    };
        //    return View(editUserViewModel);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Failed to edit profile");
        //        return View("EditUserProfile", editVM);
        //    }

        //    AppUser user = await _dashboardRespository.GetByIdNoTracking(editVM.Id);

        //    if (user == null)
        //    {
        //        return View("Error");
                
        //    }
        //    var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
        //    if (photoResult.Error != null)
        //    {
        //        ModelState.AddModelError("Image", "Failed to upload image");
        //        return View("EditUserProfile", editVM);
        //    }
        //    if (!string.IsNullOrEmpty(user.ProfileImageUrl))
        //    {
        //        _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
        //    }
        //    //Optimistic Concurrency - "Tracking Error"
        //    MapUserEdit(user, editVM, photoResult);

        //    _dashboardRespository.Update(user);

        //    return RedirectToAction("Index");
        //}
    }
}
