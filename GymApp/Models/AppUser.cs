using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymApp.Models
{
    public class AppUser : IdentityUser
    {
        public int? YearsOfExperience { get; set; }
        public string? ProfileImageUrl { get; set; }
        
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Gym>? Gyms { get; set; }
        public ICollection<WorkoutProgram>? WorkoutPrograms { get; set; }
    }
}
