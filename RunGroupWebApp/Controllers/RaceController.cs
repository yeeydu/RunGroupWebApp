using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.Repository;
using RunGroupWebApp.Services;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class RaceController : Controller
    {

        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
        }
        public async Task<IActionResult> Races()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetById(id);
            return View(race);

        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    RaceCategory = raceVM.RaceCategory,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Races");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetById(id);
            if (race == null) return View("Error");
            var raceEditVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                Url = race.Image,
                RaceCategory = race.RaceCategory,

            };
            return View(raceEditVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceEditVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceEditVM);
            }

            var userRace = await _raceRepository.GetByIdNoTracking(id);

            if (userRace == null)
            {
                return View("Error");
            }

            // Update the properties of the existing club
            userRace.Title = raceEditVM.Title;
            userRace.Description = raceEditVM.Description;
            userRace.AddressId = raceEditVM.AddressId;
            userRace.Address = raceEditVM.Address;
            userRace.RaceCategory = raceEditVM.RaceCategory;

            // Check if a new image is provided
            if (raceEditVM.Image != null)
            {
                // Optionally delete the old photo if needed
                // await _photoService.DeletePhotoAsync(userClub.Image);

                var photoResult = await _photoService.AddPhotoAsync(raceEditVM.Image);
                userRace.Image = photoResult.Url.ToString(); // Update the image URL
            }
            else
            {
                // If no new image keep the existing image
                userRace.Image = userRace.Image;
            }

            // Update the club in the repository
            _raceRepository.Update(userRace);

            return RedirectToAction("Races");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceRepository.GetById(id);
            if (raceDetails == null) return View("Error");
            return View(raceDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var raceDetails = await _raceRepository.GetById(id);
            if (raceDetails == null) return View("Error");

            _raceRepository.Delete(raceDetails);
            return RedirectToAction("Races");
        }



    }
}
