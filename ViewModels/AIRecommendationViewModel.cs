using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.ViewModels
{
    public class AIRecommendationViewModel
    {
        [Display(Name = "Vücut Tipi")]
        [StringLength(50)]
        public string? BodyType { get; set; }

        [Display(Name = "Boy (cm)")]
        [Range(50, 250, ErrorMessage = "Boy 50-250 cm arasında olmalıdır")]
        public double? Height { get; set; }

        [Display(Name = "Kilo (kg)")]
        [Range(30, 300, ErrorMessage = "Kilo 30-300 kg arasında olmalıdır")]
        public double? Weight { get; set; }

        [Display(Name = "Yaş")]
        [Range(10, 100, ErrorMessage = "Yaş 10-100 arasında olmalıdır")]
        public int? Age { get; set; }

        [Display(Name = "Hedef")]
        [StringLength(50)]
        public string? Goal { get; set; }

        [Display(Name = "Fotoğraf Yükle")]
        public IFormFile? ImageFile { get; set; }
    }
}
