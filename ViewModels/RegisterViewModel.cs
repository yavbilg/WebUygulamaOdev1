using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50)]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50)]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "?ifre zorunludur")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter olmal?d?r.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "?ifre")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "?ifre Tekrar")]
        [Compare("Password", ErrorMessage = "?ifreler e?le?miyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Do?um Tarihi")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(200)]
        [Display(Name = "Adres")]
        public string? Address { get; set; }
    }
}
