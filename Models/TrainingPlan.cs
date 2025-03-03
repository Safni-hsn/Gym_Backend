using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.Models
{
    public class TrainingPlan
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public string Title { get; set; }  // Plan title (e.g., "Weight Loss Plan")

        [Required]
        public string Description { get; set; }  // Plan details

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
