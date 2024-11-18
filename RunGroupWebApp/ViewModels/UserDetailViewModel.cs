using Microsoft.AspNetCore.Components.Routing;

namespace RunGroupWebApp.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string? UserName { get; internal set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }

        public string? Location { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
