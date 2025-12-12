using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class AIRecommendation
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public string UserId { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? BodyType { get; set; }
        
        [Range(50, 250)]
        public double? Height { get; set; }
        
        [Range(30, 300)]
        public double? Weight { get; set; }
        
        [Range(10, 100)]
        public int? Age { get; set; }
        
        [StringLength(50)]
        public string? Goal { get; set; } // Kilo verme, kas yapma, vb.
        
        public string? ImageUrl { get; set; }
        
        public string? Recommendation { get; set; }
        
        public string? ExercisePlan { get; set; }
        
        public string? DietPlan { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }
    }
}
