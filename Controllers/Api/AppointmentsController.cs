using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAppointments(
            [FromQuery] string? userId = null,
            [FromQuery] int? trainerId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] AppointmentStatus? status = null)
        {
            var query = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(a => a.UserId == userId);
            }

            if (trainerId.HasValue)
            {
                query = query.Where(a => a.TrainerId == trainerId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate <= endDate.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.Status == status.Value);
            }

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.StartTime,
                    a.EndTime,
                    a.Status,
                    a.Notes,
                    a.CreatedDate,
                    User = new
                    {
                        a.User!.Id,
                        a.User.FirstName,
                        a.User.LastName,
                        a.User.Email
                    },
                    Trainer = new
                    {
                        a.Trainer!.Id,
                        a.Trainer.FirstName,
                        a.Trainer.LastName,
                        FullName = a.Trainer.FirstName + " " + a.Trainer.LastName
                    },
                    Service = new
                    {
                        a.Service!.Id,
                        a.Service.Name,
                        a.Service.DurationMinutes,
                        a.Service.Price
                    }
                })
                .ToListAsync();

            return Ok(appointments);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                    .ThenInclude(t => t.FitnessCenter)
                .Include(a => a.Service)
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.StartTime,
                    a.EndTime,
                    a.Status,
                    a.Notes,
                    a.CreatedDate,
                    User = new
                    {
                        a.User!.Id,
                        a.User.FirstName,
                        a.User.LastName,
                        a.User.Email,
                        a.User.PhoneNumber
                    },
                    Trainer = new
                    {
                        a.Trainer!.Id,
                        a.Trainer.FirstName,
                        a.Trainer.LastName,
                        FullName = a.Trainer.FirstName + " " + a.Trainer.LastName,
                        a.Trainer.Email,
                        a.Trainer.PhoneNumber,
                        a.Trainer.Specialization
                    },
                    Service = new
                    {
                        a.Service!.Id,
                        a.Service.Name,
                        a.Service.Description,
                        a.Service.DurationMinutes,
                        a.Service.Price,
                        a.Service.ServiceType
                    },
                    FitnessCenter = new
                    {
                        a.Trainer.FitnessCenter!.Id,
                        a.Trainer.FitnessCenter.Name,
                        a.Trainer.FitnessCenter.Address,
                        a.Trainer.FitnessCenter.PhoneNumber
                    }
                })
                .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // GET: api/Appointments/Statistics
        [HttpGet("Statistics")]
        public async Task<ActionResult<object>> GetStatistics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = _context.Appointments.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.AppointmentDate <= endDate.Value);
            }

            var statistics = new
            {
                TotalAppointments = await query.CountAsync(),
                PendingAppointments = await query.CountAsync(a => a.Status == AppointmentStatus.Pending),
                ConfirmedAppointments = await query.CountAsync(a => a.Status == AppointmentStatus.Confirmed),
                CancelledAppointments = await query.CountAsync(a => a.Status == AppointmentStatus.Cancelled),
                CompletedAppointments = await query.CountAsync(a => a.Status == AppointmentStatus.Completed),
                AppointmentsByService = await query
                    .Include(a => a.Service)
                    .GroupBy(a => a.Service!.Name)
                    .Select(g => new { ServiceName = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync(),
                AppointmentsByTrainer = await query
                    .Include(a => a.Trainer)
                    .GroupBy(a => new { a.Trainer!.FirstName, a.Trainer.LastName })
                    .Select(g => new
                    {
                        TrainerName = g.Key.FirstName + " " + g.Key.LastName,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .ToListAsync()
            };

            return Ok(statistics);
        }
    }
}
