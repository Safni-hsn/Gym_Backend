using GymManagementSystem.Data;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    [Route("api/members")]
    [ApiController]
    [Authorize] // ✅ Requires Authentication
    public class MemberController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all members (Admin & Trainer only)
        [HttpGet]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<ActionResult<IEnumerable<object>>> GetMembers()
        {
            var members = await _context.Users
                .Where(u => u.Role == "Member")  // ✅ Fetch only users with Member role
                .Select(u => new
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email
                })
                .ToListAsync();

            if (members.Count == 0)
                return NotFound(new { message = "No members found" });

            return Ok(members);
        }

        // ✅ Get a single member by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Trainer,Member")] // Members can view their own profile
        public async Task<ActionResult<object>> GetMember(string id)
        {
            var member = await _context.Users
                .Where(u => u.Id == id && u.Role == "Member")
                .Select(u => new
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (member == null)
                return NotFound(new { message = "Member not found" });

            return Ok(member);
        }

        // ✅ Add a new member (Admin & Trainer only)
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> AddMember([FromBody] ApplicationUser member)
        {
            if (string.IsNullOrEmpty(member.FullName) || string.IsNullOrEmpty(member.Email))
                return BadRequest(new { message = "Full Name and Email are required." });

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == member.Email);
            if (existingUser != null)
                return BadRequest(new { message = "User with this email already exists." });

            member.Role = "Member"; // ✅ Ensure the new user is a "Member"
            _context.Users.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        // ✅ Update member details
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> UpdateMember(string id, [FromBody] ApplicationUser updatedMember)
        {
            var member = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == "Member");

            if (member == null)
                return NotFound(new { message = "Member not found." });

            // ✅ Update only allowed fields
            member.FullName = updatedMember.FullName ?? member.FullName;
            member.Email = updatedMember.Email ?? member.Email;

            _context.Users.Update(member);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Member updated successfully." });
        }

        // ✅ Delete a member (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMember(string id)
        {
            var member = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == "Member");

            if (member == null)
                return NotFound(new { message = "Member not found." });

            _context.Users.Remove(member);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Member deleted successfully." });
        }
    }
}
