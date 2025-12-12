using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using FitnessCenterApp.ViewModels;

namespace FitnessCenterApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .Where(a => a.UserId == user!.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Trainers = await _context.Trainers
                .Where(t => t.IsAvailable)
                .ToListAsync();
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var service = await _context.Services.FindAsync(model.ServiceId);

                if (service == null)
                {
                    ModelState.AddModelError("", "Seçilen hizmet bulunamad?.");
                }
                else
                {
                    var endTime = model.StartTime.Add(TimeSpan.FromMinutes(service.DurationMinutes));

                    // Check if trainer is available at this time
                    var dayOfWeek = model.AppointmentDate.DayOfWeek;
                    var isTrainerAvailable = await _context.TrainerAvailabilities
                        .AnyAsync(ta => ta.TrainerId == model.TrainerId &&
                                       ta.DayOfWeek == dayOfWeek &&
                                       ta.StartTime <= model.StartTime &&
                                       ta.EndTime >= endTime &&
                                       ta.IsAvailable);

                    if (!isTrainerAvailable)
                    {
                        ModelState.AddModelError("", "Antrenör seçilen saatte müsait de?il.");
                    }
                    else
                    {
                        // Check for conflicting appointments
                        var hasConflict = await _context.Appointments
                            .AnyAsync(a => a.TrainerId == model.TrainerId &&
                                          a.AppointmentDate == model.AppointmentDate &&
                                          a.Status != AppointmentStatus.Cancelled &&
                                          ((a.StartTime <= model.StartTime && a.EndTime > model.StartTime) ||
                                           (a.StartTime < endTime && a.EndTime >= endTime) ||
                                           (a.StartTime >= model.StartTime && a.EndTime <= endTime)));

                        if (hasConflict)
                        {
                            ModelState.AddModelError("", "Seçilen saatte çak??an bir randevu var.");
                        }
                        else
                        {
                            var appointment = new Appointment
                            {
                                UserId = user!.Id,
                                TrainerId = model.TrainerId,
                                ServiceId = model.ServiceId,
                                AppointmentDate = model.AppointmentDate,
                                StartTime = model.StartTime,
                                EndTime = endTime,
                                Notes = model.Notes,
                                Status = AppointmentStatus.Pending,
                                CreatedDate = DateTime.Now
                            };

                            _context.Appointments.Add(appointment);
                            await _context.SaveChangesAsync();

                            TempData["Success"] = "Randevunuz ba?ar?yla olu?turuldu. Onay bekliyor.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            ViewBag.Trainers = await _context.Trainers
                .Where(t => t.IsAvailable)
                .ToListAsync();
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var appointment = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == user!.Id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == user!.Id);

            if (appointment == null)
                return NotFound();

            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevunuz iptal edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimeSlots(int trainerId, DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            
            var availability = await _context.TrainerAvailabilities
                .Where(ta => ta.TrainerId == trainerId && 
                            ta.DayOfWeek == dayOfWeek && 
                            ta.IsAvailable)
                .FirstOrDefaultAsync();

            if (availability == null)
                return Json(new List<string>());

            var existingAppointments = await _context.Appointments
                .Where(a => a.TrainerId == trainerId && 
                           a.AppointmentDate == date &&
                           a.Status != AppointmentStatus.Cancelled)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToListAsync();

            var timeSlots = new List<string>();
            var currentTime = availability.StartTime;

            while (currentTime.Add(TimeSpan.FromMinutes(30)) <= availability.EndTime)
            {
                var slotEnd = currentTime.Add(TimeSpan.FromMinutes(30));
                var hasConflict = existingAppointments.Any(a => 
                    (a.StartTime <= currentTime && a.EndTime > currentTime) ||
                    (a.StartTime < slotEnd && a.EndTime >= slotEnd));

                if (!hasConflict)
                {
                    timeSlots.Add(currentTime.ToString(@"hh\:mm"));
                }

                currentTime = currentTime.Add(TimeSpan.FromMinutes(30));
            }

            return Json(timeSlots);
        }
    }
}
