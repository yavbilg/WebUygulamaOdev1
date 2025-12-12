# Fitness Center Yönetim ve Randevu Sistemi

## Proje Hakk?nda

Bu proje, ASP.NET Core MVC kullan?larak geli?tirilmi? kapsaml? bir Spor Salonu (Fitness Center) Yönetim ve Randevu Sistemidir. Sistem, spor salonlar?n?n sundu?u hizmetleri, antrenörlerin uzmanl?k alanlar?n?, üyelerin randevular?n? ve yapay zekâ tabanl? egzersiz önerilerini yönetmektedir.

## Özellikler

### Temel Özellikler
- ? Kullan?c? Kay?t ve Giri? Sistemi (Identity)
- ? Rol Bazl? Yetkilendirme (Admin ve Member rolleri)
- ? Spor Salonu Yönetimi (CRUD i?lemleri)
- ? Antrenör Yönetimi (CRUD i?lemleri)
- ? Hizmet Yönetimi (CRUD i?lemleri)
- ? Randevu Sistemi (CRUD i?lemleri)
- ? Randevu Onay Mekanizmas?
- ? Antrenör Müsaitlik Takibi
- ? Çak??an Randevu Kontrolü

### ?leri Düzey Özellikler
- ? REST API (Trainer ve Appointment API'leri)
- ? LINQ ile Filtreleme ve Raporlama
- ? AI Tabanl? Egzersiz ve Diyet Önerileri
- ? Admin Dashboard ile ?statistikler
- ? Responsive Tasar?m (Bootstrap 5)
- ? Client ve Server Side Validation

## Teknolojiler

- **Framework:** ASP.NET Core MVC (.NET 10.0)
- **Dil:** C#
- **Veritaban?:** SQL Server (LocalDB)
- **ORM:** Entity Framework Core 10.0
- **Authentication:** ASP.NET Core Identity
- **Frontend:** Bootstrap 5, Font Awesome, jQuery
- **API:** REST API with JSON responses

## Kullan?lan NuGet Paketleri

```xml
- Microsoft.EntityFrameworkCore.SqlServer (10.0.1)
- Microsoft.EntityFrameworkCore.Tools (10.0.1)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (10.0.1)
- Microsoft.AspNetCore.Authentication.JwtBearer (10.0.1)
```

## Veritaban? Modeli

### Tablolar
1. **AspNetUsers** - Kullan?c? bilgileri (Identity)
2. **AspNetRoles** - Roller (Identity)
3. **FitnessCenters** - Spor salonlar?
4. **Trainers** - Antrenörler
5. **Services** - Hizmetler
6. **TrainerServices** - Antrenör-Hizmet ili?kisi (Many-to-Many)
7. **TrainerAvailabilities** - Antrenör müsaitlik saatleri
8. **Appointments** - Randevular
9. **MembershipPlans** - Üyelik planlar?
10. **AIRecommendations** - AI önerileri

### ?li?kiler
- FitnessCenter ? Trainers (One-to-Many)
- FitnessCenter ? Services (One-to-Many)
- Trainer ? TrainerAvailabilities (One-to-Many)
- Trainer ? Appointments (One-to-Many)
- Trainer ? Services (Many-to-Many via TrainerServices)
- User ? Appointments (One-to-Many)
- User ? MembershipPlans (One-to-Many)
- User ? AIRecommendations (One-to-Many)

## Kurulum

### Gereksinimler
- .NET 10.0 SDK veya üzeri
- SQL Server LocalDB veya SQL Server
- Visual Studio 2022 veya VS Code

### Ad?mlar

1. **Projeyi Klonlay?n**
```bash
git clone https://github.com/yavbilg/WebUygulamaOdev1.git
cd WebUygulamaOdev1
```

2. **Veritaban?n? Olu?turun**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

3. **Uygulamay? Çal??t?r?n**
```bash
dotnet run
```

4. **Taray?c?da Aç?n**
```
https://localhost:5001
```

## Varsay?lan Kullan?c?lar

### Admin Hesab?
- **Email:** ogrencinumarasi@sakarya.edu.tr
- **?ifre:** sau
- **Rol:** Admin

## Kullan?m

### Üye ??lemleri
1. Kay?t ol sayfas?ndan yeni hesap olu?turun
2. Giri? yap?n
3. Randevu olu?turun
4. AI önerisi al?n
5. Randevular?n?z? görüntüleyin

### Admin ??lemleri
1. Admin hesab?yla giri? yap?n
2. Dashboard'dan istatistikleri görüntüleyin
3. Salonlar?, antrenörleri ve hizmetleri yönetin
4. Randevular? onaylay?n veya iptal edin

## API Kullan?m?

### Trainer API

**Tüm Antrenörleri Getir**
```
GET /api/trainers
```

**Filtrelenmi? Antrenörler**
```
GET /api/trainers?specialization=yoga&isAvailable=true
```

**Belirli Bir Antrenör**
```
GET /api/trainers/{id}
```

**Müsait Antrenörler**
```
GET /api/trainers/available?date=2025-01-15&startTime=10:00
```

### Appointment API

**Tüm Randevular? Getir**
```
GET /api/appointments
```

**Filtrelenmi? Randevular**
```
GET /api/appointments?trainerId=1&status=Confirmed
```

**Randevu ?statistikleri**
```
GET /api/appointments/statistics?startDate=2025-01-01&endDate=2025-01-31
```

## AI Önerileri

Sistem, kullan?c?lar?n vücut bilgilerine (boy, kilo, ya?, hedef) göre:
- BMI hesaplamas?
- Ki?iselle?tirilmi? egzersiz program?
- Beslenme önerileri
- 12 haftal?k detayl? plan

sa?lamaktad?r.

## Proje Yap?s?

```
FitnessCenterApp/
??? Controllers/
?   ??? AccountController.cs
?   ??? AdminController.cs
?   ??? AppointmentsController.cs
?   ??? AIController.cs
?   ??? HomeController.cs
?   ??? Api/
?       ??? TrainersController.cs
?       ??? AppointmentsController.cs
??? Models/
?   ??? ApplicationUser.cs
?   ??? FitnessCenter.cs
?   ??? Trainer.cs
?   ??? Service.cs
?   ??? Appointment.cs
?   ??? TrainerService.cs
?   ??? TrainerAvailability.cs
?   ??? MembershipPlan.cs
?   ??? AIRecommendation.cs
??? ViewModels/
?   ??? RegisterViewModel.cs
?   ??? LoginViewModel.cs
?   ??? AppointmentCreateViewModel.cs
?   ??? AIRecommendationViewModel.cs
??? Data/
?   ??? ApplicationDbContext.cs
??? Views/
    ??? Account/
    ??? Admin/
    ??? Appointments/
    ??? AI/
    ??? Home/
    ??? Shared/
```

## Güvenlik

- ? Password hashing (Identity)
- ? Anti-forgery tokens
- ? Role-based authorization
- ? Input validation (client & server)
- ? SQL Injection korumas? (EF Core)
- ? XSS korumas?

## Geli?tirme Notlar?

### Yap?lacaklar (Future Enhancements)
- [ ] Email bildirimleri
- [ ] SMS bildirimleri
- [ ] Online ödeme entegrasyonu
- [ ] Gerçek OpenAI API entegrasyonu
- [ ] Responsive mobil uygulama
- [ ] QR kod ile check-in
- [ ] Sosyal medya payla??m?

## Lisans

Bu proje e?itim amaçl? geli?tirilmi?tir.

## ?leti?im

**Ö?renci:** [Ad?n?z Soyad?n?z]  
**Ö?renci No:** [Numaran?z]  
**Email:** ogrencinumarasi@sakarya.edu.tr  
**GitHub:** https://github.com/yavbilg/WebUygulamaOdev1

## Te?ekkürler

Sakarya Üniversitesi - Bilgisayar Mühendisli?i Bölümü  
Web Programlama Dersi - 2025-2026 Güz Dönemi
