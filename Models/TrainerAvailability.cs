using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class TrainerAvailability
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public int TrainerId { get; set; }
        
        [Required(ErrorMessage = "Gün seçimi zorunludur")]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        public TimeSpan StartTime { get; set; }
        
        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        public TimeSpan EndTime { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        // Navigation Properties
        public virtual Trainer? Trainer { get; set; }
    }
}
