using System.ComponentModel.DataAnnotations;

namespace RunGroupWebApp.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email Address is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
