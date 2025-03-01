namespace GymManagementSystem.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string Type { get; set; } // Monthly, Yearly
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
