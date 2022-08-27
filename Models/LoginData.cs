using System.ComponentModel.DataAnnotations;

namespace MVC_test.Models
{
    public class LoginData
    {
        [Required(ErrorMessage = "Email field must not be empty")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field must not be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
