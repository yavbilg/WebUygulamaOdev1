// ===================================
// Gerekli Kütüphaneler ve Namespace'ler
// ===================================
using Microsoft.EntityFrameworkCore;  // Entity Framework Core - Veritabanı işlemleri için
using Microsoft.AspNetCore.Identity;   // Identity sistemi - Kullanıcı yönetimi ve kimlik doğrulama
using FitnessCenterApp.Data;           // Uygulama veritabanı context'i
using FitnessCenterApp.Models;         // Uygulama model sınıfları
using System.Text;                     // Encoding ayarları için

// ===================================
// Konsol ve Web Server Encoding Ayarları - Türkçe Karakter Desteği
// ===================================
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// ===================================
// WebApplication Builder Oluşturma
// ===================================
// WebApplication.CreateBuilder: ASP.NET Core uygulaması oluşturmak için temel yapıyı hazırlar
// args: Komut satırı argümanlarını alır
var builder = WebApplication.CreateBuilder(args);

// ===================================
// Servislerin Container'a Eklenmesi (Dependency Injection)
// ===================================

// MVC yapısını ekle - Controller'lar ve View'lar için gerekli
// AddControllersWithViews: Hem API hem de View döndüren controller'ları destekler
builder.Services.AddControllersWithViews();

// ===================================
// Veritabanı Context Konfigürasyonu
// ===================================
// ApplicationDbContext'i Dependency Injection container'a ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // SQL Server kullan - Connection string appsettings.json'dan alınır
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===================================
// Identity Sistemi Konfigürasyonu
// ===================================
// ASP.NET Core Identity - Kullanıcı kimlik doğrulama ve yetkilendirme sistemi
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Şifre gereksinimleri - Güvenlik politikaları
    options.Password.RequireDigit = true;              // En az 1 rakam zorunlu
    options.Password.RequireLowercase = true;          // En az 1 küçük harf zorunlu
    options.Password.RequireUppercase = true;          // En az 1 büyük harf zorunlu
    options.Password.RequireNonAlphanumeric = false;   // Özel karakter zorunlu değil
    options.Password.RequiredLength = 6;               // Minimum 6 karakter
    
    // Kullanıcı ayarları
    options.User.RequireUniqueEmail = true;            // Her email adresi benzersiz olmalı
})
.AddEntityFrameworkStores<ApplicationDbContext>()      // Identity verilerini EF Core ile sakla
.AddDefaultTokenProviders();                           // Email onayı, şifre sıfırlama için token sağlayıcılar

// ===================================
// Cookie Ayarları (Oturum Yönetimi)
// ===================================
// Kullanıcı oturumu için cookie yapılandırması
builder.Services.ConfigureApplicationCookie(options =>
{
    // Giriş sayfası yolu - Yetkisiz erişimde yönlendirilecek sayfa
    options.LoginPath = "/Account/Login";
    
    // Çıkış sayfası yolu
    options.LogoutPath = "/Account/Logout";
    
    // Erişim reddedildi sayfası - Yetkisiz işlem denemelerinde gösterilir
    options.AccessDeniedPath = "/Account/AccessDenied";
    
    // Cookie geçerlilik süresi - 24 saat
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    
    // Sliding expiration - Kullanıcı aktif olduğunda süre otomatik uzar
    options.SlidingExpiration = true;
});

// ===================================
// Uygulama Oluşturma (Build)
// ===================================
var app = builder.Build();

// ===================================
// Veritabanı Başlangıç Verilerini Oluşturma (Seed Data)
// ===================================
// Uygulama başlarken rolleri ve admin kullanıcısını oluştur
using (var scope = app.Services.CreateScope())
{
    // Servisleri scope içinden al
    var services = scope.ServiceProvider;
    
    // Rol ve admin kullanıcısı oluştur
    await SeedData(services);
    
    // Örnek verileri veritabanına ekle (salonlar, antrenörler, hizmetler vb.)
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    await DbSeeder.SeedDataAsync(dbContext);
}

// ===================================
// HTTP Request Pipeline Konfigürasyonu
// ===================================
// Gelen HTTP isteklerinin nasıl işleneceğini belirleyen middleware'ler

// Development (Geliştirme) ortamı değilse
if (!app.Environment.IsDevelopment())
{
    // Hata yakalama middleware'i - Kullanıcı dostu hata sayfası gösterir
    app.UseExceptionHandler("/Home/Error");
    
    // HTTP Strict Transport Security - HTTPS kullanımını zorlar
    app.UseHsts();
    
    // HTTP isteklerini HTTPS'e yönlendir
    app.UseHttpsRedirection();
}
else
{
    // Development ortamında detaylı hata sayfası göster
    app.UseDeveloperExceptionPage();
}

// ===================================
// Static Dosyalar Middleware'i
// ===================================
// wwwroot klasöründeki statik dosyaları servis et (CSS, JS, resimler)
app.UseStaticFiles();

// ===================================
// Routing Middleware'i
// ===================================
// URL'leri controller action'larına eşleştir
app.UseRouting();

// ===================================
// Kimlik Doğrulama ve Yetkilendirme
// ===================================
// Kimlik doğrulama middleware'i - Kullanıcının kimliğini doğrular
app.UseAuthentication();

// Yetkilendirme middleware'i - Kullanıcının erişim yetkisini kontrol eder
app.UseAuthorization();

// ===================================
// Route (Yönlendirme) Yapılandırması
// ===================================
// Default route pattern: {controller=Home}/{action=Index}/{id?}
// Örnek: /Home/Index, /Account/Login/5
app.MapControllerRoute(
    name: "default",                                    // Route adı
    pattern: "{controller=Home}/{action=Index}/{id?}"); // URL şablonu

// ===================================
// Uygulamayı Çalıştır
// ===================================
// HTTP isteklerini dinlemeye başla
app.Run();

// ===================================
// Veritabanı Başlangıç Verilerini Oluşturma Metodu
// ===================================
/// <summary>
/// Uygulama başlarken gerekli rolleri ve admin kullanıcısını oluşturur
/// </summary>
/// <param name="serviceProvider">Dependency Injection servis sağlayıcı</param>
async Task SeedData(IServiceProvider serviceProvider)
{
    // Rol yöneticisi servisi - Rolleri oluşturmak ve yönetmek için
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    // Kullanıcı yöneticisi servisi - Kullanıcıları oluşturmak ve yönetmek için
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // ===================================
    // Rolleri Oluştur
    // ===================================
    // Sistemde kullanılacak rol isimleri
    string[] roleNames = { "Admin", "Member" };
    
    // Her rol için
    foreach (var roleName in roleNames)
    {
        // Rol daha önce oluşturulmamışsa
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            // Yeni rol oluştur ve veritabanına kaydet
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // ===================================
    // Admin Kullanıcısını Oluştur
    // ===================================
    // Admin kullanıcı bilgileri
    var adminEmail = "ogrencinumarasi@sakarya.edu.tr";  // Admin email adresi
    var adminPassword = "sau";                            // Admin şifresi

    // Bu email ile kullanıcı var mı kontrol et
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    // Kullanıcı yoksa oluştur
    if (adminUser == null)
    {
        // Yeni admin kullanıcı nesnesi
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,           // Kullanıcı adı = email
            Email = adminEmail,              // Email adresi
            FirstName = "Admin",             // Ad
            LastName = "User",               // Soyad
            EmailConfirmed = true,           // Email onaylanmış olarak işaretle
            RegistrationDate = DateTime.Now  // Kayıt tarihi
        };

        // Kullanıcıyı Identity sistemine kaydet
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        
        // Kayıt başarılıysa
        if (result.Succeeded)
        {
            // Kullanıcıya Admin rolünü ata
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}
