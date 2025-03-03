using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using GymManagementSystem.Data;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    [Route("api/training-plans")]
    [ApiController]
    [Authorize]
    public class TrainingPlanController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainingPlanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Get all training plans (Members & Trainers)
        [HttpGet]
        [Authorize(Roles = "Admin,Trainer,Member")]
        public async Task<ActionResult<IEnumerable<TrainingPlan>>> GetTrainingPlans()
        {
            return await _context.TrainingPlans.ToListAsync();
        }

        // ✅ Create a new training plan (Trainer Only)
        [HttpPost]
[Authorize(Roles = "Trainer")]
public async Task<IActionResult> CreateTrainingPlan([FromBody] TrainingPlan plan)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    _context.TrainingPlans.Add(plan);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetTrainingPlans), new { id = plan.Id }, plan);
}


        // ✅ Delete a training plan (Trainers Only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Trainer")]
        public async Task<IActionResult> DeleteTrainingPlan(int id)
        {
            var plan = await _context.TrainingPlans.FindAsync(id);
            if (plan == null)
                return NotFound();

            _context.TrainingPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Training plan deleted." });
        }
    }
}
