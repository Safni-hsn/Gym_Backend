namespace GymManagementSystem.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime JoinDate { get; set; }
        public string MembershipType { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
