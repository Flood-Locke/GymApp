using System.ComponentModel.DataAnnotations;

namespace GymApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
    }
}
