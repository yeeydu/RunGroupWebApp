using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroupWebApp.Models
{

    // Identity was made to wrok with EF
    public class AppUser : IdentityUser // user key word
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; } // add the foreign key [ForeignKey("Address")]
        public Address? Address { get; set; }

        public ICollection<Club> Clubs { get; set; }

        public ICollection<Race> Races { get; set; }

    }
}
