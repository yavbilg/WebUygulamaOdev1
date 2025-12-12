using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalMembers = await _context.Users.CountAsync(),
                TotalTrainers = await _context.Trainers.CountAsync(),
                TotalAppointments = await _context.Appointments.CountAsync(),
                PendingAppointments = await _context.Appointments
                    .CountAsync(a => a.Status == AppointmentStatus.Pending)
            };
            
            ViewBag.Stats = stats;
            return View();
        }

        // Fitness Centers Management
        public async Task<IActionResult> FitnessCenters()
        {
            var centers = await _context.FitnessCenters.ToListAsync();
            return View(centers);
        }

        [HttpGet]
        public IActionResult CreateFitnessCenter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFitnessCenter(FitnessCenter model)
        {
            if (ModelState.IsValid)
            {
                _context.FitnessCenters.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FitnessCenters));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditFitnessCenter(int id)
        {
            var center = await _context.FitnessCenters.FindAsync(id);
            if (center == null)
                return NotFound();
            
            return View(center);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFitnessCenter(int id, FitnessCenter model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.FitnessCenters.AnyAsync(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(FitnessCenters));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFitnessCenter(int id)
        {
            var center = await _context.FitnessCenters.FindAsync(id);
            if (center != null)
            {
                _context.FitnessCenters.Remove(center);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(FitnessCenters));
        }

        // Trainers Management
        public async Task<IActionResult> Trainers()
        {
            var trainers = await _context.Trainers
                .Include(t => t.FitnessCenter)
                .ToListAsync();
            return View(trainers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrainer()
        {
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(Trainer model)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Trainers));
            }
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();
            
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainer(int id, Trainer model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Trainers.AnyAsync(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Trainers));
            }
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Trainers));
        }

        // Services Management
        public async Task<IActionResult> Services()
        {
            var services = await _context.Services
                .Include(s => s.FitnessCenter)
                .ToListAsync();
            return View(services);
        }

        [HttpGet]
        public async Task<IActionResult> CreateService()
        {
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateService(Service model)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
                return NotFound();
            
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditService(int id, Service model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Services.AnyAsync(e => e.Id == id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Services));
            }
            ViewBag.FitnessCenters = await _context.FitnessCenters.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Services));
        }

        // Appointments Management
        public async Task<IActionResult> Appointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Confirmed;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Appointments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Appointments));
        }
    }
}
