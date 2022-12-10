using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GymApp.ViewModels;
using GymApp.Interfaces;
using GymApp.Models;

namespace GymApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _photoService = photoService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    City = user.Address?.City,
                    Province = user.Address?.Province,
                    YearsOfExperience = user.YearsOfExperience,
                    UserName = user.UserName,
                    ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-1.jpg",
                    Gyms = user.Gyms,
                    WorkoutPrograms = user.WorkoutPrograms,
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Detail(/*string id*/)
        {
            var user = await _userManager.GetUserAsync(User);
            //var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                City = user.Address?.City,
                Province = user.Address?.Province,
                
                YearsOfExperience = user.YearsOfExperience,
                UserName = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl ?? "/img/avatar-male-1.jpg",
            };
            return View(userDetailViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            //var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return View("Error");
            }

            var editMV = new EditProfileViewModel
            {
                YearsOfExperience = user.YearsOfExperience,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.Address?.City,
                Province= user.Address?.Province,
                              
            };
            return View(editMV);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
           
            //var userProfile = await _userRepository.GetUserById(id);
          
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
                ModelState.AddModelError("", "Failed to edit profile");
                return RedirectToAction("Detail", "User");
            }

            var userProfile = await _userManager.GetUserAsync(User);

            if (userProfile == null)
            {
                return View("Error");
            }

            
            var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Failed to upload image");
                return View("EditProfile", editVM);
            }

            if (!string.IsNullOrEmpty(userProfile.ProfileImageUrl))
            {
                _ = _photoService.DeletePhotoAsync(userProfile.ProfileImageUrl);
            }
            var address = new Address
            {
                City = editVM.City,
                Province = editVM.Province,
            };

            userProfile.Address = address;           
            userProfile.YearsOfExperience = editVM.YearsOfExperience;
            userProfile.ProfileImageUrl = photoResult.Url.ToString();
            

            //var user = new AppUser
            //{
            //    //Id = id,
            //    ProfileImageUrl = photoResult.Url.ToString(),
            //    YearsOfExperience = editVM.YearsOfExperience,
            //    Address = address,
            //};

            //_userRepository.Update(user);
            await _userManager.UpdateAsync(userProfile);
            

            //return View(editVM);
            return RedirectToAction("Detail", "User", new { userProfile.Id });
        }

           

            //await _userManager.UpdateAsync(userProfile);

              
    }
}
