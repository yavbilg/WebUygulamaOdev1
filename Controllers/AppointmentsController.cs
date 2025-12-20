// Gerekli kütüphaneler
using Microsoft.AspNetCore.Authorization;      // Yetkilendirme attribute'ları için
using Microsoft.AspNetCore.Identity;          // Identity kullanıcı yönetimi için
using Microsoft.AspNetCore.Mvc;               // Controller ve Action metodları için
using Microsoft.EntityFrameworkCore;          // Entity Framework Core - LINQ sorguları için
using FitnessCenterApp.Data;                  // Veritabanı context'i
using FitnessCenterApp.Models;                // Model sınıfları
using FitnessCenterApp.ViewModels;            // ViewModel sınıfları

namespace FitnessCenterApp.Controllers
{
    /// <summary>
    /// Randevu işlemlerini yöneten Controller
    /// Randevu listeleme, oluşturma, iptal etme ve müsait saat sorgulama işlemlerini yönetir
    /// </summary>
    [Authorize]  // Bu controller'a sadece giriş yapmış kullanıcılar erişebilir
    public class AppointmentsController : Controller
    {
        // ===================================
        // Dependency Injection - Servis Referansları
        // ===================================
        
        // Veritabanı context'i - Tüm veritabanı işlemleri için
        private readonly ApplicationDbContext _context;
        
        // Kullanıcı yöneticisi - Şu anki kullanıcı bilgilerini almak için
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Constructor - Dependency Injection ile servisleri alır
        /// </summary>
        public AppointmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ===================================
        // Randevu Listesi (Index)
        // ===================================
        
        /// <summary>
        /// Kullanıcının tüm randevularını listeler
        /// Geçmiş ve gelecek randevuları tarih sırasına göre gösterir
        /// </summary>
        /// <returns>Randevu listesi view'ı</returns>
        public async Task<IActionResult> Index()
        {
            // Şu anki giriş yapmış kullanıcıyı al
            var user = await _userManager.GetUserAsync(User);
            
            // Kullanıcının randevularını veritabanından çek
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)        // Antrenör bilgilerini dahil et (JOIN)
                .Include(a => a.Service)        // Hizmet bilgilerini dahil et (JOIN)
                .Where(a => a.UserId == user!.Id)  // Sadece bu kullanıcının randevuları
                .OrderByDescending(a => a.AppointmentDate)  // Tarihe göre yeniden eskiye sırala
                .ToListAsync();                 // Listeyi asenkron olarak al

            // View'a randevu listesini gönder
            return View(appointments);
        }

        // ===================================
        // Yeni Randevu Oluşturma - GET
        // ===================================
        
        /// <summary>
        /// Yeni randevu oluşturma formunu gösterir
        /// Antrenör ve hizmet listelerini ViewBag ile view'a gönderir
        /// </summary>
        /// <returns>Randevu oluşturma formu view'ı</returns>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Aktif antrenörleri al ve ViewBag'e ekle
            ViewBag.Trainers = await _context.Trainers
                .Where(t => t.IsAvailable)      // Sadece müsait antrenörler
                .ToListAsync();
            
            // Aktif hizmetleri al ve ViewBag'e ekle
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)         // Sadece aktif hizmetler
                .ToListAsync();

            return View();
        }

        // ===================================
        // Yeni Randevu Oluşturma - POST
        // ===================================
        
        /// <summary>
        /// Yeni randevu kaydını oluşturur
        /// Antrenör müsaitliği ve çakışma kontrolü yapar
        /// </summary>
        /// <param name="model">Randevu bilgilerini içeren ViewModel</param>
        /// <returns>Başarılıysa Index'e yönlendirir, hatalıysa formu tekrar gösterir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Create(AppointmentCreateViewModel model)
        {
            // Model validasyonu - Tüm gerekli alanlar dolu mu?
            if (ModelState.IsValid)
            {
                // Şu anki kullanıcıyı al
                var user = await _userManager.GetUserAsync(User);
                
                // Seçilen hizmeti veritabanından bul
                var service = await _context.Services.FindAsync(model.ServiceId);

                // Hizmet bulunamadıysa hata ver
                if (service == null)
                {
                    ModelState.AddModelError("", "Seçilen hizmet bulunamadı.");
                }
                else
                {
                    // ===================================
                    // Bitiş Saatini Hesapla
                    // ===================================
                    // Başlangıç saati + Hizmet süresi = Bitiş saati
                    var endTime = model.StartTime.Add(TimeSpan.FromMinutes(service.DurationMinutes));

                    // ===================================
                    // Antrenör Müsaitliği Kontrolü
                    // ===================================
                    // Seçilen günün haftanın hangi günü olduğunu al (Pazartesi, Salı vb.)
                    var dayOfWeek = model.AppointmentDate.DayOfWeek;
                    
                    // Antrenörün bu günde ve bu saatte müsait olup olmadığını kontrol et
                    var isTrainerAvailable = await _context.TrainerAvailabilities
                        .AnyAsync(ta => ta.TrainerId == model.TrainerId &&      // Seçilen antrenör
                                       ta.DayOfWeek == dayOfWeek &&             // Seçilen gün
                                       ta.StartTime <= model.StartTime &&       // Başlangıç saati müsaitlik içinde
                                       ta.EndTime >= endTime &&                 // Bitiş saati müsaitlik içinde
                                       ta.IsAvailable);                         // Müsait olarak işaretlenmiş

                    // Antrenör müsait değilse hata ver
                    if (!isTrainerAvailable)
                    {
                        ModelState.AddModelError("", "Antrenör seçilen saatte müsait değil.");
                    }
                    else
                    {
                        // ===================================
                        // Çakışan Randevu Kontrolü
                        // ===================================
                        // Aynı antrenörün aynı tarih ve saatte başka randevusu var mı?
                        var hasConflict = await _context.Appointments
                            .AnyAsync(a => a.TrainerId == model.TrainerId &&                    // Aynı antrenör
                                          a.AppointmentDate == model.AppointmentDate &&         // Aynı tarih
                                          a.Status != AppointmentStatus.Cancelled &&            // İptal edilmemiş
                                          (
                                              // Çakışma senaryoları:
                                              (a.StartTime <= model.StartTime && a.EndTime > model.StartTime) ||  // Mevcut randevu yeni randevunun başlangıcını kapsıyor
                                              (a.StartTime < endTime && a.EndTime >= endTime) ||                  // Mevcut randevu yeni randevunun bitişini kapsıyor
                                              (a.StartTime >= model.StartTime && a.EndTime <= endTime)            // Yeni randevu mevcut randevuyu tamamen kapsıyor
                                          ));

                        // Çakışma varsa hata ver
                        if (hasConflict)
                        {
                            ModelState.AddModelError("", "Seçilen saatte çakışan bir randevu var.");
                        }
                        else
                        {
                            // ===================================
                            // Randevu Oluştur
                            // ===================================
                            // Yeni Appointment nesnesi oluştur
                            var appointment = new Appointment
                            {
                                UserId = user!.Id,                      // Randevuyu alan kullanıcı
                                TrainerId = model.TrainerId,            // Seçilen antrenör
                                ServiceId = model.ServiceId,            // Seçilen hizmet
                                AppointmentDate = model.AppointmentDate, // Randevu tarihi
                                StartTime = model.StartTime,            // Başlangıç saati
                                EndTime = endTime,                      // Hesaplanan bitiş saati
                                Notes = model.Notes,                    // Kullanıcı notları
                                Status = AppointmentStatus.Pending,     // Durum: Beklemede
                                CreatedDate = DateTime.Now              // Oluşturulma zamanı
                            };

                            // Randevuyu veritabanına ekle
                            _context.Appointments.Add(appointment);
                            
                            // Değişiklikleri kaydet
                            await _context.SaveChangesAsync();

                            // Başarı mesajı - TempData bir sonraki sayfada gösterilir
                            TempData["Success"] = "Randevunuz başarıyla oluşturuldu. Onay bekliyor.";
                            
                            // Randevu listesi sayfasına yönlendir
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            // Hata varsa veya validasyon başarısızsa
            // Dropdown'ları tekrar doldur
            ViewBag.Trainers = await _context.Trainers
                .Where(t => t.IsAvailable)
                .ToListAsync();
            ViewBag.Services = await _context.Services
                .Where(s => s.IsActive)
                .ToListAsync();

            // Formu hata mesajlarıyla birlikte tekrar göster
            return View(model);
        }

        // ===================================
        // Randevu Detayları
        // ===================================
        
        /// <summary>
        /// Belirli bir randevunun detaylarını gösterir
        /// </summary>
        /// <param name="id">Randevu ID'si</param>
        /// <returns>Randevu detay view'ı</returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            // Şu anki kullanıcıyı al
            var user = await _userManager.GetUserAsync(User);
            
            // Randevuyu veritabanından bul
            var appointment = await _context.Appointments
                .Include(a => a.Trainer)        // Antrenör bilgilerini dahil et
                .Include(a => a.Service)        // Hizmet bilgilerini dahil et
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == user!.Id);  // Sadece kullanıcının kendi randevusu

            // Randevu bulunamadıysa 404 döndür
            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        // ===================================
        // Randevu İptal Etme
        // ===================================
        
        /// <summary>
        /// Randevuyu iptal eder (durum Cancelled olarak değiştirilir)
        /// </summary>
        /// <param name="id">İptal edilecek randevu ID'si</param>
        /// <returns>Index sayfasına yönlendirir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            // Şu anki kullanıcıyı al
            var user = await _userManager.GetUserAsync(User);
            
            // Randevuyu bul
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == user!.Id);

            // Randevu bulunamadıysa 404 döndür
            if (appointment == null)
                return NotFound();

            // Randevu durumunu "İptal Edildi" olarak güncelle
            appointment.Status = AppointmentStatus.Cancelled;
            
            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Başarı mesajı
            TempData["Success"] = "Randevunuz iptal edildi.";
            
            // Randevu listesine dön
            return RedirectToAction(nameof(Index));
        }

        // ===================================
        // AJAX: Müsait Saat Dilimleri
        // ===================================
        
        /// <summary>
        /// Belirli bir antrenör ve tarih için müsait saat dilimlerini döndürür
        /// AJAX ile çağrılır, JSON formatında sonuç döner
        /// </summary>
        /// <param name="trainerId">Antrenör ID'si</param>
        /// <param name="date">Randevu tarihi</param>
        /// <returns>Müsait saat listesi (JSON)</returns>
        [HttpGet]
        public async Task<IActionResult> GetAvailableTimeSlots(int trainerId, DateTime date)
        {
            // Seçilen tarihin haftanın hangi günü olduğunu al
            var dayOfWeek = date.DayOfWeek;
            
            // Antrenörün bu gündeki müsaitlik bilgisini al
            var availability = await _context.TrainerAvailabilities
                .Where(ta => ta.TrainerId == trainerId &&      // Belirtilen antrenör
                            ta.DayOfWeek == dayOfWeek &&       // Belirtilen gün
                            ta.IsAvailable)                    // Müsait olarak işaretlenmiş
                .FirstOrDefaultAsync();

            // Müsaitlik bilgisi yoksa boş liste döndür
            if (availability == null)
                return Json(new List<string>());

            // Bu tarihteki mevcut randevuları al (çakışma kontrolü için)
            var existingAppointments = await _context.Appointments
                .Where(a => a.TrainerId == trainerId &&                 // Aynı antrenör
                           a.AppointmentDate == date &&                 // Aynı tarih
                           a.Status != AppointmentStatus.Cancelled)     // İptal edilmemiş
                .Select(a => new { a.StartTime, a.EndTime })            // Sadece saat bilgilerini al
                .ToListAsync();

            // ===================================
            // Saat Dilimlerini Oluştur
            // ===================================
            var timeSlots = new List<string>();
            var currentTime = availability.StartTime;  // Müsaitlik başlangıç saati

            // Müsaitlik bitiş saatine kadar 30 dakikalık dilimler oluştur
            while (currentTime.Add(TimeSpan.FromMinutes(30)) <= availability.EndTime)
            {
                // Dilimin bitiş saatini hesapla
                var slotEnd = currentTime.Add(TimeSpan.FromMinutes(30));
                
                // Bu saat diliminde çakışan randevu var mı kontrol et
                var hasConflict = existingAppointments.Any(a => 
                    // Mevcut randevu bu dilimle çakışıyor mu?
                    (a.StartTime <= currentTime && a.EndTime > currentTime) ||
                    (a.StartTime < slotEnd && a.EndTime >= slotEnd));

                // Çakışma yoksa bu saati listeye ekle
                if (!hasConflict)
                {
                    // Format: "14:30" şeklinde
                    timeSlots.Add(currentTime.ToString(@"hh\:mm"));
                }

                // Bir sonraki 30 dakikalık dilime geç
                currentTime = currentTime.Add(TimeSpan.FromMinutes(30));
            }

            // JSON formatında saat listesini döndür
            // AJAX tarafından kullanılacak
            return Json(timeSlots);
        }
    }
}
