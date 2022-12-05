using GymApp.Interfaces;
using GymApp.Models;
using GymApp.ViewModels;
using GymApp.Data.Enum;
using GymApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class GymController : Controller
    {
        private readonly IGymRepository _gymRepository;
        private readonly IPhotoService _photoService;

        public GymController(IGymRepository gymRepository, IPhotoService photoService)
        {

            _gymRepository = gymRepository;
            _photoService = photoService;
        }

        [HttpGet]
        [Route("Gyms")]
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 5)
        {
            if (page < 1 || pageSize < 1) { return NotFound(); }

            // if category is -1 (All) dont filter else filter by selected category
            var gyms = category switch
            {
                -1 => await _gymRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _gymRepository.GetGymsByCategoryAndSliceAsync((GymCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _gymRepository.GetCountAsync(),
                _ => await _gymRepository.GetCountByCategoryAsync((GymCategory)category),
            };

            var gymViewModel = new IndexGymViewModel
            {
                Gyms = gyms,
                PageSize = pageSize,
                Page = page,
                TotalGyms = count,
                Category = category,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };
            return View(gymViewModel);
        }

        [HttpGet]
        [Route("Gyms/{province}")]
        public async Task<IActionResult> ListGymsByProvince(string province)
        {
            var gyms = await _gymRepository.GetGymsByProvince(ProvinceConverter.GetProvinceByName(province).ToString());
            var gymVM = new ListGymsByProvinceViewModel()
            {
                Gyms = gyms
            };
            if (gyms.Count() == 0)
            {
                gymVM.NoGymWarning = true;
            }
            else
            {
                gymVM.Province = province;
            }
            return View(gymVM);
        }

        [HttpGet]
        [Route("Gyms/{city}/{province}")]
        public async Task<IActionResult> ListGymsByCity(string city, string province)
        {
            var gyms = await _gymRepository.GetGymByCity(city);
            var gymVM = new ListGymsByCityViewModel()
            {
                Gyms = gyms
            };
            if (gyms.Count() == 0)
            {
                gymVM.NoGymWarning = true;
            }
            else
            {
                gymVM.Province = province;
                gymVM.City = city;
            }
            return View(gymVM);
        }

        [HttpGet]
        [Route("Gyms/Detail/{id}")]

        public async Task<IActionResult> Detail(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            return gym == null ? NotFound() : View(gym);
        }

        [HttpGet]
        [Route("Gyms/Province")]
        public async Task<IActionResult> GymsByProvinceDirectory()
        {
            var province = await _gymRepository.GetAllProvinces();
            var gymVM = new GymsByProvince()
            {
                Province = province
            };

            return province == null ? NotFound() : View(gymVM);
        }

        [HttpGet]
        [Route("Gyms/Province/City")]
        public async Task<IActionResult> GymsByProvinceForCityDirectory()
        {
            var province = await _gymRepository.GetAllProvinces();
            var gymVM = new GymsByProvince()
            {
                Province = province
            };

            return province == null ? NotFound() : View(gymVM);
        }

        [HttpGet]
        [Route("Gyms/{province}/City")]
        public async Task<IActionResult> GymsByCityDirectory(string province)
        {
            var cities = await _gymRepository.GetAllCitiesByProvince(ProvinceConverter.GetProvinceByName(province).ToString());
            var gymVM = new GymsByCity()
            {
                Cities = cities
            };

            return cities == null ? NotFound() : View(gymVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = HttpContext.User.GetUserId();
            var createGymViewModel = new CreateGymViewModel { AppUserId = curUserId };
            return View(createGymViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGymViewModel gymVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(gymVM.Image);
                var gym = new Gym
                {
                    Title = gymVM.Title,
                    Description = gymVM.Description,
                    Image = result.Url.ToString(),
                    GymCategory = gymVM.GymCategory,
                    AppUserId = gymVM.AppUserId,
                    Address = new Address
                    {
                        Street = gymVM.Address.Street,
                        City = gymVM.Address.City,
                        Province = gymVM.Address.Province,
                    }
                };
                _gymRepository.Add(gym);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            return View(gymVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var gym = await _gymRepository.GetByIdAsync(id);
            if (gym == null) return View("Error");
            var clubVM = new EditGymViewModel
            {
                Title = gym.Title,
                Description = gym.Description,
                AddressId = gym.AddressId,
                Address = gym.Address,
                URL = gym.Image,
                GymCategory = gym.GymCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditGymViewModel gymVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit gym");
                return View("Edit", gymVM);
            }

            var userGym = await _gymRepository.GetByIdAsyncNoTracking(id);

            if (userGym == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(gymVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(gymVM);
            }

            if (!string.IsNullOrEmpty(userGym.Image))
            {
                _ = _photoService.DeletePhotoAsync(userGym.Image);
            }

            var gym = new Gym
            {
                Id = id,
                Title = gymVM.Title,
                Description = gymVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = gymVM.AddressId,
                Address = gymVM.Address,
            };

            _gymRepository.Update(gym);

            return RedirectToAction("Index");
        }
    
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var gymDetails = await _gymRepository.GetByIdAsync(id);

            if (gymDetails == null) return View("Error");
            
            return View(gymDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteGym(int id)
        {
            var gymDetails = await _gymRepository.GetByIdAsync(id);
            
            if (gymDetails == null) return View("Error");
            if (!string.IsNullOrEmpty(gymDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(gymDetails.Image);
            }
            _gymRepository.Delete(gymDetails);
            return RedirectToAction("Index");
        }
    }
}
