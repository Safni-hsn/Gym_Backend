using GymManagementSystem.Data;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize] // ✅ Requires Authentication
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all payments (Admin only)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.Include(p => p.Member).ToListAsync();
        }

        // ✅ Get payment by ID (Admin & Member)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Member")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.Include(p => p.Member)
                                                 .FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
                return NotFound();

            return payment;
        }

        // ✅ Add a new payment (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
        }

        // ✅ Update payment details (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] Payment payment)
        {
            if (id != payment.Id)
                return BadRequest();

            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Delete a payment (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
