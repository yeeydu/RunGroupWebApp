using RunGroupWebApp.Models;

namespace RunGroupWebApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<Race> Races { get; set; } // explicit define what to want
        public List<Club> Clubs { get; set; }
    }
}
