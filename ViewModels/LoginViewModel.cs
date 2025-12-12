using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "?ifre zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "?ifre")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Beni Hat?rla")]
        public bool RememberMe { get; set; }
    }
}
