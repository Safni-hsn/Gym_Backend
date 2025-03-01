namespace GymManagementSystem.Models
{
    public class RegisterModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // Admin, Trainer, Member
        public string Password { get; set; }
    }
}
