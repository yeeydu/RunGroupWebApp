using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {

        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService) // ctor import repository  
        {

            _clubRepository = clubRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Clubs()  // Task<IActionResult> for async
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetById(id);  // Include a very expensive database call
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    ClubCategory = clubVM.ClubCategory,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Clubs");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetById(id);
            if (club == null) return View("Error"); 
            var clubEditVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                Url = club.Image,
                ClubCategory = club.ClubCategory,

            };
            return View(clubEditVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubEditVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit Club");
                return View("Edit", clubEditVM);
            }

            var userClub = await _clubRepository.GetByIdNoTracking(id);

            if (userClub == null)
            {
                return View("Error");
            }

            // Update the properties of the existing club
            userClub.Title = clubEditVM.Title;
            userClub.Description = clubEditVM.Description;
            userClub.AddressId = clubEditVM.AddressId;
            userClub.Address = clubEditVM.Address;
            userClub.ClubCategory = clubEditVM.ClubCategory;

            // Check if a new image is provided
            if (clubEditVM.Image != null)
            {
                // Optionally delete the old photo if needed
                // await _photoService.DeletePhotoAsync(userClub.Image);

                var photoResult = await _photoService.AddPhotoAsync(clubEditVM.Image);
                userClub.Image = photoResult.Url.ToString(); // Update the image URL
            }
            else
            {
                // If no new image keep the existing image
                userClub.Image = userClub.Image; 
            }

            // Update the club in the repository
            _clubRepository.Update(userClub);

            return RedirectToAction("Clubs");


        }

        public async Task<IActionResult> Delete (int id)
        {
            var clubDetails = await _clubRepository.GetById(id);
            if(clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetById(id);
            if (clubDetails == null) return View("Error");

            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Clubs");
        }


    }
}
