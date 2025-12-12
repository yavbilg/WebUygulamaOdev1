using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class TrainerService
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public int TrainerId { get; set; }
        public int ServiceId { get; set; }
        
        // Navigation Properties
        public virtual Trainer? Trainer { get; set; }
        public virtual Service? Service { get; set; }
    }
}
