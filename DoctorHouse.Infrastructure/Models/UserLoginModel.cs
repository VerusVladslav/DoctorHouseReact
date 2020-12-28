using System.ComponentModel.DataAnnotations;

namespace DoctorHouse.Infrastructure.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}