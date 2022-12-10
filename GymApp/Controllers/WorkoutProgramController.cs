using GymApp.Data;
using GymApp.Interfaces;
using GymApp.Models;
using GymApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Controllers
{
    public class WorkoutProgramController : Controller
    {
        private readonly IWorkoutProgramRepository _workoutProgramRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkoutProgramController(IWorkoutProgramRepository workoutProgramRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _workoutProgramRepository = workoutProgramRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<WorkoutProgram> workoutPrograms = await _workoutProgramRepository.GetAll();
            return View(workoutPrograms);
        }
        public async Task<IActionResult> Detail(int id)
        {
            WorkoutProgram workoutProgram = await _workoutProgramRepository.GetByIdAsync(id);
            return View(workoutProgram);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createWorkoutProgramViewModel = new CreateWorkoutProgramViewModel { AppUserId = curUser };
            return View(createWorkoutProgramViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkoutProgramViewModel workoutVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(workoutVM.Image);
                var workoutProgram = new WorkoutProgram
                {
                    Title = workoutVM.Title,
                    Description = workoutVM.Description,
                    Image = result.Url.ToString(),
                    WorkoutProgramCategory = workoutVM.WorkoutProgramCategory,
                    AppUserId = workoutVM.AppUserId,                   
                };
                _workoutProgramRepository.Add(workoutProgram);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            return View(workoutVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var workoutPrograms = await _workoutProgramRepository.GetByIdAsync(id);
            if (workoutPrograms == null) return View("Error");
            var workoutProgramVM = new EditWorkoutProgramViewModel
            {
                Title = workoutPrograms.Title,
                Description = workoutPrograms.Description,
                URL = workoutPrograms.Image,
                //AddressId = workoutPrograms.AddressId,
                WorkoutProgramCategory = workoutPrograms.WorkoutProgramCategory
            };
            return View(workoutProgramVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditWorkoutProgramViewModel workoutProgramVM)
        {
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("", "Failed to edit workout program");
                return View(workoutProgramVM);
            }



            var userWorkoutProgram = await _workoutProgramRepository.GetByIdAsyncNoTracking(id);

            if (userWorkoutProgram == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(workoutProgramVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(workoutProgramVM);
            }

            if (!string.IsNullOrEmpty(userWorkoutProgram.Image))
            {
                _ = _photoService.DeletePhotoAsync(userWorkoutProgram.Image);
            }
            var workoutProgram = new WorkoutProgram
            {
                Id = id,
                Title = workoutProgramVM.Title,
                Description = workoutProgramVM.Description,
                Image = photoResult.Url.ToString(),
                
            };

            _workoutProgramRepository.Update(workoutProgram);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var workoutProgramDetails = await _workoutProgramRepository.GetByIdAsync(id);
            if (workoutProgramDetails == null) return View("Error");
            return View(workoutProgramDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteGym(int id)
        {
            var workoutProgramDetails = await _workoutProgramRepository.GetByIdAsync(id);
            if (workoutProgramDetails == null) return View("Error");
            if (!string.IsNullOrEmpty(workoutProgramDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(workoutProgramDetails.Image);
            }
            
            _workoutProgramRepository.Delete(workoutProgramDetails);
            return RedirectToAction("Index");
        }
    }
}
