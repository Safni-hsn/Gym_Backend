using GymManagementSystem.Data;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _context.Members.ToListAsync();
        }

        // ✅ Get a single member by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Trainer,Member")] // Members can view their own profile
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound();
            return member;
        }

        // ✅ Add a new member (Admin & Trainer only)
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> AddMember([FromBody] Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        // ✅ Update member details
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Trainer")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member member)
        {
            if (id != member.Id)
                return BadRequest();

            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Delete a member (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
