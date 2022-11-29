using GymApp.Helpers;
using GymApp.Interfaces;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace GymApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGymRepository _gymRepository;

        public HomeController(ILogger<HomeController> logger, IGymRepository gymRepository)
        {
            _logger = logger;
            _gymRepository = gymRepository;
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
                else
                {
                    homeViewModel.Gyms = null;
                }
                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                homeViewModel.Gyms = null;
            }
            return View(homeViewModel);
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
