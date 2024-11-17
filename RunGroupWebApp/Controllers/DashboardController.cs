﻿using Microsoft.AspNetCore.Mvc;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(  IDashboardRepository dashboardRepository)
        {
 
            _dashboardRepository = dashboardRepository;
        }
        public  async Task<IActionResult> Index()
        {
            var useRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();

            var userViewModel = new DashboardViewModel()
            {
                Races = useRaces,
                Clubs = userClubs
            };

            return View(DashboardViewModel);
        }
    }
}
