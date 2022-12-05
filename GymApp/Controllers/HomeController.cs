using GymApp.Helpers;
using GymApp.Interfaces;
using GymApp.Models;
using GymApp.ViewModels;
using GymApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using GymmApp.ViewModels;

namespace GymApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IGymRepository _gymRepository;

        public HomeController(ILogger<HomeController> logger, IGymRepository gymRepository, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _gymRepository = gymRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            //Fetching data from other website may fail do to network, etc
            try
            {
                string url = "https://ipinfo.io?token=1b5ea1d36304f9";
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);//Take JSON and turning it into an object
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.Province = ipInfo.Region;

                if (homeViewModel.City != null)
                {
                    homeViewModel.Gyms = await _gymRepository.GetGymByCity(homeViewModel.City);
                }
                return View(homeViewModel);
            }
            catch (Exception)
            {
                homeViewModel.Gyms = null;
            }
            return View(homeViewModel);
        }
        //        public IActionResult Register()
        //        {
        //            var response = new HomeUserCreateViewModel();
        //            return View(response);
        //        }

        //        [HttpPost]
        //        public async Task<IActionResult> Register(HomeUserCreateViewModel createVM)
        //        {
        //            if (!ModelState.IsValid) return View(createVM);
        //            var user = await _userManager.FindByEmailAsync(createVM.Email);
        //            if (user != null)
        //            {
        //                ModelState.AddModelError("Register.Email", "This email is already in user");
        //                return View(createVM);
        //            }
        //            var newUser = new AppUser
        //            {
        //                UserName = createVM.UserName,
        //                Email = createVM.Email,
        //            };
        //            var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);
        //            if (newUserResponse.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(newUser, isPersistent: false);
        //                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
        //            }
        //            return RedirectToAction("Index", "Gym");
        //        }

        //        public IActionResult Privacy()
        //        {
        //            return View();
        //        }
        //        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //        public IActionResult Error()
        //        {
        //            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //        }
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel homeVM)
        {
            var createVM = homeVM.Register;

            if (!ModelState.IsValid) return View(homeVM);

            var user = await _userManager.FindByEmailAsync(createVM.Email);
            if (user != null)
            {
                ModelState.AddModelError("Register.Email", "This email is already in user");
                return View(homeVM);
            }

            var newUser = new AppUser
            {
                UserName = createVM.UserName,
                Email = createVM.Email,
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, createVM.Password);

            if (newUserResponse.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return RedirectToAction("Index", "Gym");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
