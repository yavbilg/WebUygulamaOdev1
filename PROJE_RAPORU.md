# F?TNESS CENTER YÖNET?M S?STEM?
## PROJE RAPORU

---

### PROJE B?LG?LER?

**Proje Ad?:** Fitness Center Yönetim ve Randevu Sistemi  
**Ders:** Web Programlama  
**Dönem:** 2025-2026 Güz Dönemi  

**Ö?renci Bilgileri:**  
- **Ad Soyad:** [ADINIZ SOYADINIZ]  
- **Ö?renci No:** [Ö?RENC? NUMARANIZ]  
- **Ders Grubu:** [DERS GRUBUNUZ]  

**GitHub Ba?lant?s?:** https://github.com/yavbilg/WebUygulamaOdev1

**Teslim Tarihi:** [TESL?M TAR?H?]

---

## 1. PROJE TANITIMI

Bu proje, spor salonlar?n?n günlük operasyonlar?n? dijitalle?tirmek ve üyelere modern bir randevu sistemi sunmak amac?yla geli?tirilmi?tir. ASP.NET Core MVC mimarisi kullan?larak olu?turulan sistem, hem yöneticilerin hem de üyelerin ihtiyaçlar?n? kar??lamaktad?r.

### 1.1. Projenin Amac?
- Spor salonu yönetimini kolayla?t?rmak
- Üyelere online randevu sistemi sunmak
- Antrenör müsaitlik takibi yapmak
- AI destekli ki?iselle?tirilmi? fitness programlar? olu?turmak
- Raporlama ve analiz imkan? sa?lamak

### 1.2. Hedef Kullan?c?lar
- **Yöneticiler (Admin):** Sistem yönetimi, salon, antrenör ve hizmet yönetimi
- **Üyeler (Member):** Randevu olu?turma, AI önerisi alma, randevu takibi

---

## 2. KULLANILAN TEKNOLOJ?LER

### 2.1. Backend Teknolojiler
- **Framework:** ASP.NET Core MVC (.NET 10.0)
- **Programlama Dili:** C# 13.0
- **ORM:** Entity Framework Core 10.0
- **Veritaban?:** SQL Server (LocalDB)
- **Authentication:** ASP.NET Core Identity
- **API:** RESTful Web API

### 2.2. Frontend Teknolojiler
- **CSS Framework:** Bootstrap 5
- **Icons:** Font Awesome 6.4
- **JavaScript:** jQuery 3.x
- **Template Engine:** Razor Views

### 2.3. NuGet Paketleri
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="10.0.1" />
</ItemGroup>
```

---

## 3. VER?TABANI MODEL?

### 3.1. Entity Relationship Diagram (ERD)

```
[AspNetUsers] 1---* [Appointments] *---1 [Trainers]
      |                                      |
      |                                      |
      1                                      *
      |                                      |
[MembershipPlans]                    [TrainerServices]
      |                                      |
      |                                      *
      1                                      |
      |                                  [Services]
[AIRecommendations]                        |
                                            1
                                            |
                                     [FitnessCenters]
```

### 3.2. Tablolar ve ?li?kiler

#### 3.2.1. AspNetUsers (Identity)
- Kullan?c? bilgilerini saklar
- FirstName, LastName, PhoneNumber, Address, DateOfBirth
- ASP.NET Core Identity ile entegre

#### 3.2.2. FitnessCenters
- Spor salonu bilgileri
- Name, Address, PhoneNumber, Email
- OpeningTime, ClosingTime
- ?li?kiler: Trainers (1-*), Services (1-*)

#### 3.2.3. Trainers
- Antrenör bilgileri
- FirstName, LastName, Email, PhoneNumber
- Specialization, ExperienceYears, Bio
- ?li?kiler: FitnessCenter (FK), Appointments (1-*), TrainerServices (*)

#### 3.2.4. Services
- Hizmet bilgileri
- Name, Description, ServiceType
- DurationMinutes, Price
- ?li?kiler: FitnessCenter (FK), Appointments (1-*), TrainerServices (*)

#### 3.2.5. Appointments
- Randevu bilgileri
- AppointmentDate, StartTime, EndTime
- Status (Pending, Confirmed, Cancelled, Completed)
- ?li?kiler: User (FK), Trainer (FK), Service (FK)

#### 3.2.6. TrainerServices
- Many-to-Many ili?ki tablosu
- Trainer ve Service aras?nda ba?lant?

#### 3.2.7. TrainerAvailabilities
- Antrenör müsaitlik saatleri
- DayOfWeek, StartTime, EndTime
- ?li?kiler: Trainer (FK)

#### 3.2.8. MembershipPlans
- Üyelik planlar?
- PlanName, StartDate, EndDate, Price
- ?li?kiler: User (FK)

#### 3.2.9. AIRecommendations
- AI öneri kay?tlar?
- BodyType, Height, Weight, Age, Goal
- Recommendation, ExercisePlan, DietPlan
- ?li?kiler: User (FK)

---

## 4. S?STEM ÖZELL?KLER?

### 4.1. Kullan?c? Yönetimi
- ? Kay?t olma (Register)
- ? Giri? yapma (Login)
- ? Rol bazl? yetkilendirme (Admin, Member)
- ? Client & Server side validation

### 4.2. Admin Paneli
- ? Dashboard ile istatistikler
- ? Fitness Center CRUD i?lemleri
- ? Trainer CRUD i?lemleri
- ? Service CRUD i?lemleri
- ? Randevu yönetimi ve onaylama

### 4.3. Randevu Sistemi
- ? Randevu olu?turma
- ? Antrenör müsaitlik kontrolü
- ? Çak??ma kontrolü
- ? Randevu listeleme ve detaylar?
- ? Randevu iptal etme
- ? Onay mekanizmas?

### 4.4. REST API
- ? Trainers API
  - GET /api/trainers (filtreleme ile)
  - GET /api/trainers/{id}
  - GET /api/trainers/available
- ? Appointments API
  - GET /api/appointments (filtreleme ile)
  - GET /api/appointments/{id}
  - GET /api/appointments/statistics
- ? LINQ ile filtreleme ve raporlama

### 4.5. AI Entegrasyonu
- ? Kullan?c? bilgilerine göre öneri
- ? BMI hesaplama
- ? Ki?iselle?tirilmi? egzersiz program?
- ? Beslenme plan? olu?turma
- ? Foto?raf yükleme özelli?i

---

## 5. EKRAN GÖRÜNTÜLER?

### 5.1. Ana Sayfa
[BURAYA ANA SAYFA EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- Responsive tasar?m
- Salon bilgileri
- Hizmet tan?t?mlar?
- Call-to-action butonlar?

### 5.2. Kay?t Olma Sayfas?
[BURAYA KAYIT SAYFASI EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- Form validation
- Kullan?c? bilgileri giri?i
- Bootstrap form bile?enleri

### 5.3. Giri? Yapma Sayfas?
[BURAYA G?R?? SAYFASI EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- Email/Password giri?i
- Beni hat?rla özelli?i
- Demo hesap bilgileri

### 5.4. Admin Dashboard
[BURAYA ADMIN DASHBOARD EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- ?statistik kartlar?
- H?zl? eri?im butonlar?
- Grafik ve raporlar

### 5.5. Randevu Olu?turma
[BURAYA RANDEVU OLU?TURMA EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- Antrenör seçimi
- Hizmet seçimi
- Tarih ve saat seçimi
- AJAX ile müsait saatleri getirme
- Randevu özeti

### 5.6. AI Öneri Sayfas?
[BURAYA AI ÖNER? SAYFASI EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- Kullan?c? bilgileri formu
- Foto?raf yükleme
- Hedef seçimi

### 5.7. AI Öneri Sonucu
[BURAYA AI ÖNER? SONUCU EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- BMI analizi
- Egzersiz program?
- Beslenme plan?
- 12 haftal?k detayl? program

### 5.8. API Test (Postman)
[BURAYA POSTMAN API TEST EKRAN GÖRÜNTÜSÜNÜ EKLEY?N]

Özellikler:
- GET /api/trainers
- JSON response
- Filtreleme örnekleri

---

## 6. PROJE YAPISI

```
WebUygulamaOdev1/
?
??? Controllers/
?   ??? AccountController.cs         # Kay?t/Giri?
?   ??? AdminController.cs            # Admin paneli
?   ??? AppointmentsController.cs     # Randevu yönetimi
?   ??? AIController.cs               # AI önerileri
?   ??? HomeController.cs             # Ana sayfa
?   ??? Api/
?       ??? TrainersController.cs     # Trainers API
?       ??? AppointmentsController.cs # Appointments API
?
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
?
??? ViewModels/
?   ??? RegisterViewModel.cs
?   ??? LoginViewModel.cs
?   ??? AppointmentCreateViewModel.cs
?   ??? AIRecommendationViewModel.cs
?
??? Data/
?   ??? ApplicationDbContext.cs
?
??? Views/
?   ??? Account/
?   ?   ??? Login.cshtml
?   ?   ??? Register.cshtml
?   ?   ??? AccessDenied.cshtml
?   ??? Admin/
?   ?   ??? Index.cshtml
?   ?   ??? Trainers.cshtml
?   ?   ??? CreateTrainer.cshtml
?   ??? Appointments/
?   ?   ??? Index.cshtml
?   ?   ??? Create.cshtml
?   ??? AI/
?   ?   ??? Recommendation.cshtml
?   ?   ??? RecommendationResult.cshtml
?   ??? Home/
?   ?   ??? Index.cshtml
?   ??? Shared/
?       ??? _Layout.cshtml
?
??? wwwroot/
?   ??? css/
?   ??? js/
?   ??? lib/
?   ??? uploads/
?
??? Migrations/
??? Program.cs
??? appsettings.json
??? README.md
??? .gitignore
```

---

## 7. KURULUM VE ÇALI?TIRMA

### 7.1. Gereksinimler
- .NET 10.0 SDK
- SQL Server LocalDB
- Visual Studio 2022 veya VS Code

### 7.2. Kurulum Ad?mlar?

1. Projeyi klonlay?n:
```bash
git clone https://github.com/yavbilg/WebUygulamaOdev1.git
```

2. Veritaban?n? olu?turun:
```bash
dotnet ef database update
```

3. Projeyi çal??t?r?n:
```bash
dotnet run
```

4. Taray?c?da aç?n:
```
https://localhost:5001
```

### 7.3. Demo Hesaplar
- **Admin:** ogrencinumarasi@sakarya.edu.tr / sau
- **Member:** Kay?t ol sayfas?ndan olu?turabilirsiniz

---

## 8. API KULLANIMI

### 8.1. Trainers API

**Tüm antrenörleri getir:**
```
GET /api/trainers
```

**Uzmanl?k alan?na göre filtrele:**
```
GET /api/trainers?specialization=yoga
```

**Müsait antrenörleri getir:**
```
GET /api/trainers/available?date=2025-01-15&startTime=10:00
```

### 8.2. Appointments API

**Tüm randevular? getir:**
```
GET /api/appointments
```

**Kullan?c?ya göre filtrele:**
```
GET /api/appointments?userId=[USER_ID]
```

**?statistikleri getir:**
```
GET /api/appointments/statistics
```

---

## 9. GÜVENL?K ÖZELL?KLER?

- ? ASP.NET Core Identity ile güvenli authentication
- ? Password hashing
- ? Role-based authorization
- ? Anti-forgery tokens
- ? HTTPS zorunlulu?u
- ? SQL Injection korumas? (EF Core)
- ? XSS korumas?

---

## 10. GELECEKTEK? GEL??T?RMELER

- Email bildirimleri
- SMS bildirimleri
- Online ödeme entegrasyonu
- Gerçek OpenAI API entegrasyonu
- Mobil uygulama
- QR kod ile check-in sistemi
- Sosyal medya entegrasyonu
- Detayl? raporlama ve grafikler

---

## 11. KAR?ILA?ILAN ZORLUKLAR VE ÇÖZÜMLER

### 11.1. Çak??an Randevu Kontrolü
**Zorluk:** Ayn? antrenör için ayn? saatte birden fazla randevu olu?turulmamas? gerekiyordu.

**Çözüm:** LINQ sorgular? ile mevcut randevular? kontrol eden bir mekanizma olu?turuldu.

### 11.2. Antrenör Müsaitlik Takibi
**Zorluk:** Antrenörlerin farkl? günlerde farkl? saatlerde müsait olmas? durumu.

**Çözüm:** TrainerAvailability tablosu olu?turularak her gün için ayr? müsaitlik tan?mland?.

### 11.3. AI Entegrasyonu
**Zorluk:** Gerçek OpenAI API kullan?m?n?n maliyet ve karma??kl?k getirmesi.

**Çözüm:** Mock AI implementation ile benzer sonuçlar üreten bir sistem geli?tirildi.

---

## 12. SONUÇ

Bu proje, Web Programlama dersinde ö?renilen tüm konular? kapsayan kapsaml? bir uygulama olarak geli?tirilmi?tir. ASP.NET Core MVC mimarisi, Entity Framework Core, Identity sistemi, RESTful API tasar?m? ve modern frontend teknolojileri ba?ar?yla entegre edilmi?tir.

Proje geli?tirme sürecinde:
- MVC pattern anlay??? peki?tirildi
- ORM kullan?m? ö?renildi
- Authentication ve Authorization kavramlar? uyguland?
- REST API tasar?m? yap?ld?
- LINQ sorgular? kullan?ld?
- Responsive tasar?m ilkeleri uyguland?

---

## 13. KAYNAKLAR

- Microsoft ASP.NET Core Documentation: https://docs.microsoft.com/aspnet/core
- Entity Framework Core Documentation: https://docs.microsoft.com/ef/core
- Bootstrap Documentation: https://getbootstrap.com/docs
- Stack Overflow: https://stackoverflow.com
- GitHub: https://github.com

---

## 14. TE?EKKÜR

Bu projenin geli?tirilmesinde destekleri için Web Programlama dersi hocam?za ve Sakarya Üniversitesi Bilgisayar Mühendisli?i Bölümü'ne te?ekkür ederiz.

---

**Tarih:** [TAR?H]  
**?mza:** [?MZA]
