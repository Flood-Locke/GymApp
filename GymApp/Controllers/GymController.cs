using GymApp.Interfaces;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.Controllers
{
    public class GymController : Controller
    {
        private readonly IGymRepository _gymRepository;
        private readonly IPhotoService _photoService;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public GymController(IGymRepository gymRepository, IPhotoService photoService)
        {

            _gymRepository = gymRepository;
            _photoService = photoService;
            //_httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            IEnumerable<Gym> gyms = await _gymRepository.GetAll();
            var gymViewModel = new IndexGymViewModel(gyms, pageNumber, 5);
            return View(gymViewModel);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Gym gym = await _gymRepository.GetByIdAsync(id);
            return View(gym);
        }

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

            if (userGym != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userGym.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(gymVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(gymVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(gymVM);
                }

                //if (!string.IsNullOrEmpty(userGym.Image))
                //{
                //    _ = _photoService.DeletePhotoAsync(userGym.Image);
                //}

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
            else
            {
                return View(gymVM);
            }
        }

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
            
            _gymRepository.Delete(gymDetails);
            return RedirectToAction("Index");
        }
    }
}
