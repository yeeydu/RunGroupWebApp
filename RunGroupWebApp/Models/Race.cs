using RunGroupWebApp.Data.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.Models
{
    public class Race
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int AddressId { get; set; } // good practice to add the field key when having relationship, to manipulate the model

        public Address? Address { get; set; }

        public RaceCategory RaceCategory { get; set; }
        [ForeignKey("AppUser")]

        public string? AppUserId { get; set; }

        public AppUser? AppUser { get; set; }

    }
}
