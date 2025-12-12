using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }

    public class Appointment
    {
        public int Id { get; set; }
        
        // Foreign Keys
        [Required(ErrorMessage = "Üye seçimi zorunludur")]
        public string UserId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Antrenör seçimi zorunludur")]
        public int TrainerId { get; set; }
        
        [Required(ErrorMessage = "Hizmet seçimi zorunludur")]
        public int ServiceId { get; set; }
        
        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        public DateTime AppointmentDate { get; set; }
        
        [Required(ErrorMessage = "Ba?lang?ç saati zorunludur")]
        public TimeSpan StartTime { get; set; }
        
        [Required(ErrorMessage = "Biti? saati zorunludur")]
        public TimeSpan EndTime { get; set; }
        
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        
        public string? Notes { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }
        public virtual Trainer? Trainer { get; set; }
        public virtual Service? Service { get; set; }
    }
}
