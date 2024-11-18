﻿namespace RunGroupWebApp.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string? Username { get; internal set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public IFormFile Image { get; set; }
    }
}
