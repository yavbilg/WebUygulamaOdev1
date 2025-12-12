using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTrainers(
            [FromQuery] string? specialization = null,
            [FromQuery] int? fitnessCenterId = null,
            [FromQuery] bool? isAvailable = null)
        {
            var query = _context.Trainers
                .Include(t => t.FitnessCenter)
                .AsQueryable();

            if (!string.IsNullOrEmpty(specialization))
            {
                query = query.Where(t => t.Specialization.Contains(specialization));
            }

            if (fitnessCenterId.HasValue)
            {
                query = query.Where(t => t.FitnessCenterId == fitnessCenterId.Value);
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(t => t.IsAvailable == isAvailable.Value);
            }

            var trainers = await query
                .Select(t => new
                {
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    FullName = t.FirstName + " " + t.LastName,
                    t.Email,
                    t.PhoneNumber,
                    t.Specialization,
                    t.ExperienceYears,
                    t.IsAvailable,
                    FitnessCenter = new
                    {
                        t.FitnessCenter!.Id,
                        t.FitnessCenter.Name
                    }
                })
                .ToListAsync();

            return Ok(trainers);
        }

        // GET: api/Trainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTrainer(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.FitnessCenter)
                .Include(t => t.Availabilities)
                .Include(t => t.TrainerServices)
                    .ThenInclude(ts => ts.Service)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    FullName = t.FirstName + " " + t.LastName,
                    t.Email,
                    t.PhoneNumber,
                    t.Specialization,
                    t.Bio,
                    t.ProfileImageUrl,
                    t.ExperienceYears,
                    t.IsAvailable,
                    FitnessCenter = new
                    {
                        t.FitnessCenter!.Id,
                        t.FitnessCenter.Name,
                        t.FitnessCenter.Address
                    },
                    Availabilities = t.Availabilities.Select(a => new
                    {
                        a.Id,
                        a.DayOfWeek,
                        a.StartTime,
                        a.EndTime,
                        a.IsAvailable
                    }),
                    Services = t.TrainerServices.Select(ts => new
                    {
                        ts.Service!.Id,
                        ts.Service.Name,
                        ts.Service.Description,
                        ts.Service.DurationMinutes,
                        ts.Service.Price
                    })
                })
                .FirstOrDefaultAsync();

            if (trainer == null)
            {
                return NotFound();
            }

            return Ok(trainer);
        }

        // GET: api/Trainers/Available
        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableTrainers(
            [FromQuery] DateTime date,
            [FromQuery] TimeSpan startTime)
        {
            var dayOfWeek = date.DayOfWeek;

            var availableTrainers = await _context.Trainers
                .Include(t => t.FitnessCenter)
                .Include(t => t.Availabilities)
                .Where(t => t.IsAvailable &&
                           t.Availabilities.Any(a => a.DayOfWeek == dayOfWeek &&
                                                     a.StartTime <= startTime &&
                                                     a.EndTime >= startTime.Add(TimeSpan.FromMinutes(30)) &&
                                                     a.IsAvailable))
                .Select(t => new
                {
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    FullName = t.FirstName + " " + t.LastName,
                    t.Specialization,
                    t.ExperienceYears,
                    FitnessCenter = new
                    {
                        t.FitnessCenter!.Id,
                        t.FitnessCenter.Name
                    }
                })
                .ToListAsync();

            return Ok(availableTrainers);
        }
    }
}
