using GymManagementSystem.Data;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    [Route("api/trainers")]
    [ApiController]
    [Authorize(Roles = "Admin")] // ✅ Only Admin can manage trainers
    public class TrainerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
        {
            return await _context.Trainers.ToListAsync();
        }

        // ✅ Get a trainer by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Trainer>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();
            return trainer;
        }

        // ✅ Add a new trainer
        [HttpPost]
        public async Task<IActionResult> AddTrainer([FromBody] Trainer trainer)
        {
            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTrainer), new { id = trainer.Id }, trainer);
        }

        // ✅ Update trainer details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, [FromBody] Trainer trainer)
        {
            if (id != trainer.Id)
                return BadRequest();

            _context.Entry(trainer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Delete a trainer
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
