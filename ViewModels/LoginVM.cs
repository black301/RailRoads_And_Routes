using System.ComponentModel.DataAnnotations;

namespace Transport__system_prototype.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
