using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Uzmanl?k alan? zorunludur")]
        [StringLength(200)]
        public string Specialization { get; set; } = string.Empty;
        
        public string? Bio { get; set; }
        
        public string? ProfileImageUrl { get; set; }
        
        [Range(0, 50)]
        public int ExperienceYears { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        // Foreign Keys
        public int FitnessCenterId { get; set; }
        
        // Navigation Properties
        public virtual FitnessCenter? FitnessCenter { get; set; }
        public virtual ICollection<TrainerAvailability> Availabilities { get; set; } = new List<TrainerAvailability>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
    }
}
