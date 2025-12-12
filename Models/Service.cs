using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Hizmet ad? zorunludur")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Aç?klama zorunludur")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Süre zorunludur")]
        [Range(15, 240, ErrorMessage = "Süre 15-240 dakika aras?nda olmal?d?r")]
        public int DurationMinutes { get; set; }
        
        [Required(ErrorMessage = "Ücret zorunludur")]
        [Range(0, 10000, ErrorMessage = "Ücret 0-10000 TL aras?nda olmal?d?r")]
        public decimal Price { get; set; }
        
        [StringLength(50)]
        public string ServiceType { get; set; } = string.Empty; // Fitness, Yoga, Pilates, etc.
        
        public bool IsActive { get; set; } = true;
        
        // Foreign Keys
        public int FitnessCenterId { get; set; }
        
        // Navigation Properties
        public virtual FitnessCenter? FitnessCenter { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
    }
}
