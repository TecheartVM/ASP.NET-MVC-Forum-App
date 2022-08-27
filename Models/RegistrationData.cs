using System.ComponentModel.DataAnnotations;

namespace MVC_test.Models
{
    public class RegistrationData
    {
        [Required(ErrorMessage = "Email field must not be empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username field must not be empty")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password field must not be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required")]
        [Compare("Password", ErrorMessage = "Password must match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
