using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.ViewModels
{
    public class AppointmentCreateViewModel
    {
        [Required(ErrorMessage = "Antrenör seçimi zorunludur")]
        [Display(Name = "Antrenör")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Hizmet seçimi zorunludur")]
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Tarih")]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Saat seçimi zorunludur")]
        [Display(Name = "Saat")]
        public TimeSpan StartTime { get; set; }

        [Display(Name = "Notlar")]
        public string? Notes { get; set; }
    }
}
