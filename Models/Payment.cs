namespace GymManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } // Paid, Pending, Failed
    }
}
