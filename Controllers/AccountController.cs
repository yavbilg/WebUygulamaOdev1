// Gerekli kütüphaneleri projeye dahil et
using Microsoft.AspNetCore.Identity;  // Kimlik doğrulama ve kullanıcı yönetimi için
using Microsoft.AspNetCore.Mvc;        // Controller ve Action metodları için
using FitnessCenterApp.Models;        // Uygulama model sınıfları
using FitnessCenterApp.ViewModels;    // View ile veri alışverişi için ViewModel'lar

namespace FitnessCenterApp.Controllers
{
    /// <summary>
    /// Kullanıcı hesap işlemlerini yöneten Controller sınıfı
    /// Kayıt olma, giriş yapma, çıkış yapma ve erişim kontrolü işlemlerini yönetir
    /// </summary>
    public class AccountController : Controller
    {
        // === Dependency Injection ile Servis Referansları === //
        
        // Kullanıcı yönetimi için Identity servisi - Kullanıcı CRUD işlemleri
        private readonly UserManager<ApplicationUser> _userManager;
        
        // Oturum yönetimi için Identity servisi - Giriş/çıkış işlemleri
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        // Rol yönetimi için Identity servisi - Admin, Member gibi roller
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Constructor - Dependency Injection ile gerekli servisleri alır
        /// </summary>
        /// <param name="userManager">Kullanıcı işlemleri için servis</param>
        /// <param name="signInManager">Oturum işlemleri için servis</param>
        /// <param name="roleManager">Rol işlemleri için servis</param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // === Kayıt Olma (Register) İşlemleri === //

        /// <summary>
        /// Kayıt formu sayfasını gösterir (GET isteği)
        /// </summary>
        /// <returns>Register view sayfası</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Yeni kullanıcı kaydını gerçekleştirir (POST isteği)
        /// Kullanıcı bilgilerini alır, doğrular ve veritabanına kaydeder
        /// </summary>
        /// <param name="model">Kayıt formundan gelen kullanıcı bilgileri</param>
        /// <returns>Başarılıysa ana sayfa, hatalıysa kayıt formu</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF saldırılarına karşı koruma
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Model doğrulama - Tüm gerekli alanlar dolu ve kurallara uygun mu?
            if (ModelState.IsValid)
            {
                // Yeni ApplicationUser nesnesi oluştur - Identity kullanıcı modeli
                var user = new ApplicationUser
                {
                    UserName = model.Email,              // Kullanıcı adı olarak email kullan
                    Email = model.Email,                 // E-posta adresi
                    FirstName = model.FirstName,         // Ad
                    LastName = model.LastName,           // Soyad
                    PhoneNumber = model.PhoneNumber,     // Telefon numarası
                    DateOfBirth = model.DateOfBirth,     // Doğum tarihi
                    Address = model.Address,             // Adres bilgisi
                    RegistrationDate = DateTime.Now      // Kayıt tarihi - Şu anki zaman
                };

                // Kullanıcıyı Identity sistemine kaydet - Şifre hash'lenerek saklanır
                var result = await _userManager.CreateAsync(user, model.Password);

                // Kayıt başarılı mı kontrol et
                if (result.Succeeded)
                {
                    // Varsayılan olarak "Member" rolünü ata
                    // Member rolü sistemde tanımlı mı kontrol et
                    if (await _roleManager.RoleExistsAsync("Member"))
                    {
                        // Kullanıcıya Member rolünü ata
                        await _userManager.AddToRoleAsync(user, "Member");
                    }

                    // Kullanıcıyı otomatik olarak sisteme giriş yaptır
                    // isPersistent: false = "Beni hatırla" özelliği kapalı
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    // Ana sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }

                // Kayıt başarısız - Hataları ModelState'e ekle
                foreach (var error in result.Errors)
                {
                    // Her bir hatayı model doğrulama hatalarına ekle
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Hata varsa veya model geçersizse, formu tekrar göster
            return View(model);
        }

        // === Giriş Yapma (Login) İşlemleri === //

        /// <summary>
        /// Giriş formu sayfasını gösterir (GET isteği)
        /// </summary>
        /// <param name="returnUrl">Giriş sonrası dönülecek sayfa URL'i (opsiyonel)</param>
        /// <returns>Login view sayfası</returns>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // ReturnUrl'i ViewData'ya ekle - View'da kullanılabilir
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Kullanıcı girişini gerçekleştirir (POST isteği)
        /// E-posta ve şifre ile doğrulama yapar
        /// </summary>
        /// <param name="model">Giriş formundan gelen email ve şifre</param>
        /// <param name="returnUrl">Başarılı giriş sonrası yönlendirilecek sayfa</param>
        /// <returns>Başarılıysa returnUrl veya ana sayfa, hatalıysa login formu</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF koruması
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            // ReturnUrl'i ViewData'ya ekle
            ViewData["ReturnUrl"] = returnUrl;

            // Model doğrulama kontrolü
            if (ModelState.IsValid)
            {
                // Kullanıcı giriş denemesi yap
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,           // Kullanıcı email'i
                    model.Password,        // Şifre
                    model.RememberMe,      // "Beni Hatırla" seçeneği
                    lockoutOnFailure: false);  // Başarısız denemede hesabı kilitleme

                // Giriş başarılı mı?
                if (result.Succeeded)
                {
                    // ReturnUrl varsa ve güvenli bir URL ise oraya yönlendir
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    // ReturnUrl yoksa ana sayfaya yönlendir
                    return RedirectToAction("Index", "Home");
                }

                // Giriş başarısız - Hata mesajı ekle
                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            }

            // Hata varsa login formunu tekrar göster
            return View(model);
        }

        // === Çıkış Yapma (Logout) İşlemi === //

        /// <summary>
        /// Kullanıcının sistemden çıkış yapmasını sağlar (POST isteği)
        /// </summary>
        /// <returns>Ana sayfaya yönlendirme</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]  // CSRF koruması
        public async Task<IActionResult> Logout()
        {
            // Kullanıcının oturumunu kapat
            await _signInManager.SignOutAsync();
            
            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }

        // === Erişim Reddedildi (Access Denied) Sayfası === //

        /// <summary>
        /// Yetkisiz erişim denemelerinde gösterilen sayfa (GET isteği)
        /// Kullanıcı yetki gerektiren bir sayfaya erişmeye çalıştığında gösterilir
        /// </summary>
        /// <returns>AccessDenied view sayfası</returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
