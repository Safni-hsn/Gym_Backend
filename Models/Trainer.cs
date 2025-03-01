namespace GymManagementSystem.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Specialty { get; set; }
        public List<Member> AssignedMembers { get; set; }
    }
}
