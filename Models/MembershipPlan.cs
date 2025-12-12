using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    public class MembershipPlan
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public string UserId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Plan ad? zorunludur")]
        [StringLength(100)]
        public string PlanName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Ba?lang?ç tarihi zorunludur")]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "Biti? tarihi zorunludur")]
        public DateTime EndDate { get; set; }
        
        [Required(ErrorMessage = "Ücret zorunludur")]
        [Range(0, 50000)]
        public decimal Price { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual ApplicationUser? User { get; set; }
    }
}
