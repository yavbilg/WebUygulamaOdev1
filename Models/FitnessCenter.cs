using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class FitnessCenter
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Salon adı zorunludur")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Adres zorunludur")]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;
        
        [Phone]
        public string? PhoneNumber { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Açılış saati zorunludur")]
        public TimeSpan OpeningTime { get; set; }
        
        [Required(ErrorMessage = "Kapanış saati zorunludur")]
        public TimeSpan ClosingTime { get; set; }
        
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
