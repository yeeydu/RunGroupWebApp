using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {

            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        // ImageUploadResult an object from cloudinary
        private void MapUserEdit(AppUser user, EditUserViewModel editUserVM, ImageUploadResult photoResult)
        {
            user.Id = editUserVM.Id;
            user.Pace = editUserVM?.Pace;
            user.Mileage = editUserVM?.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editUserVM.City;
            user.State = editUserVM?.State;

        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();

            var userViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };

            return View(userViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
            if (user == null) return View("Error");

            var editUserViewModel = new EditUserViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,
            };
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserViewModel editUserVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit user");
                return View("EditUserProfile", editUserVM);
            }

            AppUser user = await _dashboardRepository.GetByIdNoTracking(editUserVM.Id);
            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);

                MapUserEdit(user, editUserVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editUserVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);
                MapUserEdit(user, editUserVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }

        }

    }
}
