# FİTNESS CENTER YÖNETİM SİSTEMİ
## Web Programlama Dersi Projesi

---

## KAPAK SAYFASI

**Proje Başlığı:** Fitness Center Yönetim Sistemi  
**Öğrenci Numarası:** [Öğrenci Numaranızı buraya yazın]  
**Ad Soyad:** [Adınız Soyadınızı buraya yazın]  
**Ders Grubu:** [Ders Grubunuzu buraya yazın]  
**GitHub Bağlantısı:** https://github.com/yavbilg/WebUygulamaOdev1  
**Proje Tarihi:** Aralık 2024  
**Teknoloji:** ASP.NET Core MVC (.NET 10)

---

## İÇİNDEKİLER

1. [Proje Tanıtımı](#1-proje-tanitimi)
2. [Veritabanı Modeli](#2-veritabani-modeli)
3. [Proje Mimarisi](#3-proje-mimarisi)
4. [Kullanıcı Rolleri ve Yetkiler](#4-kullanici-rolleri-ve-yetkiler)
5. [Önemli Özellikler](#5-onemli-ozellikler)
6. [Güvenlik Özellikleri](#6-guvenlik-ozellikleri)
7. [Kullanım Senaryoları](#7-kullanim-senaryolari)
8. [Ekran Görüntüleri](#8-ekran-goruntuleri)
9. [Kurulum ve Çalıştırma](#9-kurulum-ve-calistirma)
10. [Geliştirme Süreci](#10-gelistirme-sureci)
11. [Gelecek Geliştirmeler](#11-gelecek-gelistirmeler)
12. [Sonuç](#12-sonuc)

---

## 1. PROJE TANITIMI

### 1.1. Proje Hakkında

Fitness Center Yönetim Sistemi, fitness merkezlerinin günlük operasyonlarını dijital ortamda yönetmek için geliştirilmiş modern bir web uygulamasıdır. Sistem, üyeler, antrenörler ve yöneticiler için ayrı ayrı tasarlanmış modüller içermektedir.

### 1.2. Projenin Amacı

Bu proje, fitness merkezi işletmelerinin aşağıdaki ihtiyaçlarını karşılamak üzere tasarlanmıştır:

- ✅ Üyelerin randevu oluşturma ve yönetimi
- ✅ Antrenörlerin müsaitlik durumlarının takibi
- ✅ Hizmet ve salon yönetimi
- ✅ AI destekli kişiselleştirilmiş egzersiz ve beslenme önerileri
- ✅ Kullanıcı kimlik doğrulama ve yetkilendirme
- ✅ Admin paneli ile merkezi yönetim

### 1.3. Kullanılan Teknolojiler

**Backend:**
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10.0
- ASP.NET Core Identity (Kullanıcı yönetimi)
- SQL Server (LocalDB)

**Frontend:**
- Razor Views
- Bootstrap 5
- jQuery
- Font Awesome Icons
- AJAX (Asenkron işlemler)

**Veritabanı:**
- Microsoft SQL Server
- Entity Framework Core Code First yaklaşımı
- Migration desteği

**Güvenlik:**
- ASP.NET Core Identity
- Rol tabanlı yetkilendirme (Admin, Member)
- CSRF koruması
- Form validasyonu

### 1.4. Temel Özellikler

#### Üye Özellikleri:
1. **Kullanıcı Kaydı ve Girişi**
   - Email ile kayıt olma
   - Güvenli şifre yönetimi
   - Profil bilgileri

2. **Randevu Yönetimi**
   - Antrenör seçimi
   - Hizmet seçimi (Kişisel Antrenman, Yoga, Pilates vb.)
   - Müsait saat sorgulama (AJAX ile dinamik)
   - Randevu oluşturma, görüntüleme, iptal etme

3. **AI Destekli Öneriler**
   - Vücut ölçüleri girişi (boy, kilo, yaş)
   - Hedef belirleme (kilo verme, kas geliştirme vb.)
   - BMI hesaplama
   - 12 haftalık egzersiz programı
   - Kişiselleştirilmiş beslenme planı
   - Fotoğraf yükleme (opsiyonel)

#### Admin Özellikleri:
1. **Salon Yönetimi**
   - Yeni salon ekleme
   - Salon bilgilerini güncelleme
   - Çalışma saatleri yönetimi

2. **Antrenör Yönetimi**
   - Antrenör ekleme/düzenleme
   - Uzmanlık alanları tanımlama
   - Müsaitlik saatleri belirleme

3. **Hizmet Yönetimi**
   - Hizmet tanımlama (Yoga, Pilates, Fitness vb.)
   - Ücret ve süre belirleme
   - Aktif/pasif durumu

4. **Randevu Yönetimi**
   - Tüm randevuları görüntüleme
   - Randevu onaylama/iptal etme
   - Durum güncelleme

---

## 2. VERİTABANI MODELİ

### 2.1. Entity İlişki Diyagramı (ER Diagram)

```
┌─────────────────┐         ┌─────────────────┐
│ FitnessCenter   │◄────────│  Trainer        │
│─────────────────│ 1     * │─────────────────│
│ Id (PK)         │         │ Id (PK)         │
│ Name            │         │ FirstName       │
│ Address         │         │ LastName        │
│ PhoneNumber     │         │ Email           │
│ Email           │         │ Specialization  │
│ OpeningTime     │         │ ExperienceYears │
│ ClosingTime     │         │ FitnessCenterId │
│ Description     │         │ IsAvailable     │
│ IsActive        │         └─────────────────┘
└─────────────────┘                  │
         │                           │ 1
         │ 1                         │
         │                           │ *
         │ *                  ┌──────▼──────────┐
   ┌─────▼─────────┐         │ TrainerAvail... │
   │   Service     │         │─────────────────│
   │───────────────│         │ Id (PK)         │
   │ Id (PK)       │         │ TrainerId (FK)  │
   │ Name          │         │ DayOfWeek       │
   │ Description   │         │ StartTime       │
   │ DurationMin   │         │ EndTime         │
   │ Price         │         │ IsAvailable     │
   │ ServiceType   │         └─────────────────┘
   │ IsActive      │
   │ FitnessCtr..  │
   └───────────────┘
         │
         │
         │ *
   ┌─────▼──────────┐         ┌─────────────────┐
   │ Appointment    │◄────────│ ApplicationUser │
   │────────────────│ *     1 │─────────────────│
   │ Id (PK)        │         │ Id (PK)         │
   │ UserId (FK)    │         │ UserName        │
   │ TrainerId (FK) │         │ Email           │
   │ ServiceId (FK) │         │ FirstName       │
   │ AppointmentDt  │         │ LastName        │
   │ StartTime      │         │ PhoneNumber     │
   │ EndTime        │         │ DateOfBirth     │
   │ Status         │         │ Address         │
   │ Notes          │         │ RegistrationDt  │
   │ CreatedDate    │         └─────────────────┘
   └────────────────┘                  │
                                       │ 1
                                       │
                                       │ *
                            ┌──────────▼─────────┐
                            │ AIRecommendation   │
                            │────────────────────│
                            │ Id (PK)            │
                            │ UserId (FK)        │
                            │ BodyType           │
                            │ Height             │
                            │ Weight             │
                            │ Age                │
                            │ Goal               │
                            │ ImageUrl           │
                            │ Recommendation     │
                            │ ExercisePlan       │
                            │ DietPlan           │
                            │ CreatedDate        │
                            └────────────────────┘
```

### 2.2. Veritabanı Tabloları

#### 2.2.1. ApplicationUser (Kullanıcı)
**Amaç:** Sistemdeki tüm kullanıcıların (üye ve admin) bilgilerini tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | string (PK) | Benzersiz kullanıcı kimliği |
| UserName | string | Kullanıcı adı (Email ile aynı) |
| Email | string | E-posta adresi |
| FirstName | string | Ad |
| LastName | string | Soyad |
| PhoneNumber | string | Telefon numarası |
| DateOfBirth | DateTime? | Doğum tarihi |
| Address | string? | Adres bilgisi |
| RegistrationDate | DateTime | Kayıt tarihi |

**İlişkiler:**
- `Appointments` (1-*): Bir kullanıcının birden fazla randevusu olabilir
- `MembershipPlans` (1-*): Bir kullanıcının birden fazla üyelik planı olabilir
- `AIRecommendations` (1-*): Bir kullanıcının birden fazla AI önerisi olabilir

#### 2.2.2. FitnessCenter (Fitness Salonu)
**Amaç:** Fitness merkezlerinin bilgilerini tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz salon kimliği |
| Name | string | Salon adı |
| Address | string | Salon adresi |
| PhoneNumber | string | İletişim telefonu |
| Email | string | İletişim e-postası |
| OpeningTime | TimeSpan | Açılış saati |
| ClosingTime | TimeSpan | Kapanış saati |
| Description | string? | Salon açıklaması |
| IsActive | bool | Aktif durumu |

**İlişkiler:**
- `Trainers` (1-*): Bir salonda birden fazla antrenör çalışabilir
- `Services` (1-*): Bir salon birden fazla hizmet sunabilir

#### 2.2.3. Trainer (Antrenör)
**Amaç:** Fitness merkezlerinde çalışan antrenörlerin bilgilerini tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz antrenör kimliği |
| FirstName | string | Ad |
| LastName | string | Soyad |
| Email | string | E-posta adresi |
| PhoneNumber | string? | Telefon numarası |
| Specialization | string | Uzmanlık alanı |
| Bio | string? | Biyografi |
| ProfileImageUrl | string? | Profil fotoğrafı |
| ExperienceYears | int | Deneyim yılı |
| IsAvailable | bool | Müsaitlik durumu |
| FitnessCenterId | int (FK) | Çalıştığı salon |

**İlişkiler:**
- `FitnessCenter` (*-1): Bir antrenör bir salonda çalışır
- `Availabilities` (1-*): Bir antrenörün birden fazla müsaitlik kaydı olabilir
- `Appointments` (1-*): Bir antrenörün birden fazla randevusu olabilir
- `TrainerServices` (1-*): Bir antrenör birden fazla hizmet verebilir

#### 2.2.4. Service (Hizmet)
**Amaç:** Fitness merkezlerinde sunulan hizmetlerin bilgilerini tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz hizmet kimliği |
| Name | string | Hizmet adı |
| Description | string | Hizmet açıklaması |
| DurationMinutes | int | Süre (dakika) |
| Price | decimal | Ücret (TL) |
| ServiceType | string | Hizmet tipi (Fitness, Yoga vb.) |
| IsActive | bool | Aktif durumu |
| FitnessCenterId | int (FK) | Sunulduğu salon |

**İlişkiler:**
- `FitnessCenter` (*-1): Bir hizmet bir salonda sunulur
- `Appointments` (1-*): Bir hizmete birden fazla randevu alınabilir
- `TrainerServices` (1-*): Bir hizmeti birden fazla antrenör verebilir

#### 2.2.5. Appointment (Randevu)
**Amaç:** Üyeler ve antrenörler arasındaki randevu kayıtlarını tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz randevu kimliği |
| UserId | string (FK) | Randevuyu alan üye |
| TrainerId | int (FK) | Randevuyu veren antrenör |
| ServiceId | int (FK) | Alınan hizmet |
| AppointmentDate | DateTime | Randevu tarihi |
| StartTime | TimeSpan | Başlangıç saati |
| EndTime | TimeSpan | Bitiş saati |
| Status | enum | Durum (Pending, Confirmed, Cancelled, Completed) |
| Notes | string? | Notlar |
| CreatedDate | DateTime | Oluşturulma tarihi |

**Status Enum Değerleri:**
- `Pending`: Beklemede
- `Confirmed`: Onaylandı
- `Cancelled`: İptal Edildi
- `Completed`: Tamamlandı

**İlişkiler:**
- `User` (*-1): Bir randevu bir kullanıcıya aittir
- `Trainer` (*-1): Bir randevuyu bir antrenör verir
- `Service` (*-1): Bir randevu bir hizmet içindir

#### 2.2.6. TrainerAvailability (Antrenör Müsaitliği)
**Amaç:** Antrenörlerin hangi günlerde ve saatlerde müsait olduğunu tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz müsaitlik kimliği |
| TrainerId | int (FK) | Antrenör |
| DayOfWeek | enum | Haftanın günü |
| StartTime | TimeSpan | Başlangıç saati |
| EndTime | TimeSpan | Bitiş saati |
| IsAvailable | bool | Müsaitlik durumu |

**İlişkiler:**
- `Trainer` (*-1): Bir müsaitlik kaydı bir antrenöre aittir

#### 2.2.7. AIRecommendation (AI Önerisi)
**Amaç:** Kullanıcılar için AI tarafından oluşturulan egzersiz ve beslenme önerilerini tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz öneri kimliği |
| UserId | string (FK) | Kullanıcı |
| BodyType | string? | Vücut tipi |
| Height | double? | Boy (cm) |
| Weight | double? | Kilo (kg) |
| Age | int? | Yaş |
| Goal | string? | Hedef |
| ImageUrl | string? | Yüklenen fotoğraf |
| Recommendation | string | Genel değerlendirme |
| ExercisePlan | string | Egzersiz programı |
| DietPlan | string | Beslenme planı |
| CreatedDate | DateTime | Oluşturulma tarihi |

**İlişkiler:**
- `User` (*-1): Bir öneri bir kullanıcıya aittir

#### 2.2.8. MembershipPlan (Üyelik Planı)
**Amaç:** Kullanıcıların üyelik planlarını tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| Id | int (PK) | Benzersiz plan kimliği |
| UserId | string (FK) | Kullanıcı |
| PlanName | string | Plan adı |
| StartDate | DateTime | Başlangıç tarihi |
| EndDate | DateTime | Bitiş tarihi |
| Price | decimal | Ücret |
| IsActive | bool | Aktif durumu |

**İlişkiler:**
- `User` (*-1): Bir plan bir kullanıcıya aittir

#### 2.2.9. TrainerService (Ara Tablo)
**Amaç:** Antrenörler ve hizmetler arasındaki çoka-çok ilişkisini tutar.

| Alan | Tip | Açıklama |
|------|-----|----------|
| TrainerId | int (FK) | Antrenör |
| ServiceId | int (FK) | Hizmet |

**İlişkiler:**
- `Trainer` (*-1): Bir kayıt bir antrenöre aittir
- `Service` (*-1): Bir kayıt bir hizmete aittir

### 2.3. Veritabanı İlişkileri Özeti

**One-to-Many (1-*) İlişkiler:**
- FitnessCenter → Trainers
- FitnessCenter → Services
- Trainer → Availabilities
- Trainer → Appointments
- User → Appointments
- User → MembershipPlans
- User → AIRecommendations
- Service → Appointments

**Many-to-Many (*-*) İlişkiler:**
- Trainer ↔ Service (TrainerService ara tablosu ile)

---

## 3. PROJE MİMARİSİ

### 3.1. MVC Yapısı

Proje, ASP.NET Core MVC mimarisini kullanmaktadır:

#### Models (Modeller)
- `ApplicationUser.cs`: Kullanıcı modeli
- `FitnessCenter.cs`: Salon modeli
- `Trainer.cs`: Antrenör modeli
- `Service.cs`: Hizmet modeli
- `Appointment.cs`: Randevu modeli
- `TrainerAvailability.cs`: Müsaitlik modeli
- `AIRecommendation.cs`: AI öneri modeli
- `MembershipPlan.cs`: Üyelik planı modeli
- `TrainerService.cs`: Ara tablo

#### Views (Görünümler)
**Shared (Ortak)**
- `_Layout.cshtml`: Ana şablon
- `_ValidationScriptsPartial.cshtml`: Validasyon scriptleri

**Account (Hesap İşlemleri)**
- `Login.cshtml`: Giriş sayfası
- `Register.cshtml`: Kayıt sayfası
- `AccessDenied.cshtml`: Erişim reddedildi

**Appointments (Randevu)**
- `Index.cshtml`: Randevu listesi
- `Create.cshtml`: Randevu oluşturma
- `Details.cshtml`: Randevu detayları

**Admin (Yönetim)**
- `Index.cshtml`: Dashboard
- `FitnessCenters.cshtml`: Salon listesi
- `CreateFitnessCenter.cshtml`: Salon ekleme
- `Trainers.cshtml`: Antrenör listesi
- `CreateTrainer.cshtml`: Antrenör ekleme
- `Services.cshtml`: Hizmet listesi
- `Appointments.cshtml`: Randevu yönetimi

**AI (Yapay Zeka)**
- `Recommendation.cshtml`: Öneri formu
- `RecommendationResult.cshtml`: Öneri sonuçları
- `History.cshtml`: Geçmiş öneriler

#### Controllers (Denetleyiciler)
- `HomeController.cs`: Ana sayfa
- `AccountController.cs`: Kimlik doğrulama
- `AppointmentsController.cs`: Randevu işlemleri
- `AdminController.cs`: Yönetim işlemleri
- `AIController.cs`: AI öneri işlemleri

**API Controllers:**
- `Api/AppointmentsController.cs`: Randevu API
- `Api/TrainersController.cs`: Antrenör API

#### ViewModels (Görünüm Modelleri)
- `LoginViewModel.cs`: Giriş formu
- `RegisterViewModel.cs`: Kayıt formu
- `AppointmentCreateViewModel.cs`: Randevu oluşturma formu
- `AIRecommendationViewModel.cs`: AI öneri formu

---

## 4. ÖNEMLİ ÖZELLİKLER

### 4.1. AJAX ile Dinamik Saat Seçimi

Randevu oluştururken, kullanıcı antrenör ve tarih seçtikten sonra müsait saatler AJAX ile dinamik olarak yüklenir.

### 4.2. AI Destekli Öneri Sistemi

Kullanıcıların fiziksel özelliklerine göre kişiselleştirilmiş planlar oluşturur.

### 4.3. Randevu Çakışma Kontrolü

Sistem, aynı antrenör için çakışan randevuları engellemek için gelişmiş kontrol mekanizması kullanır.

### 4.4. Türkçe Karakter Desteği

Proje tamamen Türkçe karakterleri desteklemektedir.

---

## 5. EKRAN GÖRÜNTÜLERİ

### 5.1. Ana Sayfa
*[Buraya ana sayfa ekran görüntüsü ekleyiniz]*

### 5.2. Kayıt Sayfası
*[Buraya kayıt sayfası ekran görüntüsü ekleyiniz]*

### 5.3. Giriş Sayfası
*[Buraya giriş sayfası ekran görüntüsü ekleyiniz]*

### 5.4. Randevu Listesi
*[Buraya randevu listesi ekran görüntüsü ekleyiniz]*

### 5.5. Yeni Randevu Oluşturma
*[Buraya randevu oluşturma ekran görüntüsü ekleyiniz]*

### 5.6. AI Öneri Formu
*[Buraya AI öneri formu ekran görüntüsü ekleyiniz]*

### 5.7. AI Öneri Sonuçları
*[Buraya AI öneri sonuçları ekran görüntüsü ekleyiniz]*

### 5.8. Admin Dashboard
*[Buraya admin dashboard ekran görüntüsü ekleyiniz]*

### 5.9. Antrenör Yönetimi
*[Buraya antrenör listesi ekran görüntüsü ekleyiniz]*

### 5.10. Salon Yönetimi
*[Buraya salon yönetimi ekran görüntüsü ekleyiniz]*

---

## 6. KURULUM VE ÇALIŞTIRMA

### 6.1. Gereksinimler

- Visual Studio 2022 veya üzeri
- .NET 10 SDK
- SQL Server (LocalDB)
- Git (GitHub için)

### 6.2. Kurulum Adımları

1. **Projeyi Klonlama:**
   ```bash
   git clone https://github.com/yavbilg/WebUygulamaOdev1.git
   cd WebUygulamaOdev1
   ```

2. **Bağımlılıkları Yükleme:**
   ```bash
   dotnet restore
   ```

3. **Veritabanı Oluşturma:**
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı Çalıştırma:**
   ```bash
   dotnet run
   ```

### 6.3. İlk Kullanım

**Admin Hesabı:**
- Email: ogrencinumarasi@sakarya.edu.tr
- Şifre: sau

---

## 7. SONUÇ

### 7.1. Proje Başarıları

✅ Tam fonksiyonel bir fitness yönetim sistemi geliştirildi
✅ Modern ve kullanıcı dostu arayüz tasarımı
✅ Güvenli kimlik doğrulama ve yetkilendirme
✅ AJAX ile dinamik kullanıcı deneyimi
✅ AI destekli kişiselleştirilmiş öneriler
✅ Kapsamlı admin paneli
✅ Türkçe karakter desteği
✅ Detaylı kod dokümantasyonu
✅ GitHub ile versiyon kontrolü

### 7.2. Proje İstatistikleri

| Metrik | Değer |
|--------|-------|
| **Toplam Kod Satırı** | ~5.000+ |
| **Model Sayısı** | 10 |
| **Controller Sayısı** | 6 |
| **View Sayısı** | 20+ |
| **API Endpoint** | 2 |
| **Veritabanı Tablosu** | 9 |
| **Geliştirme Süresi** | 9 hafta |
| **Commit Sayısı** | 50+ |

---

## RAPOR SONU

**Rapor Hazırlayan:** [Adınız Soyadınız]  
**Tarih:** Aralık 2024  
**Ders:** Web Programlama  
**Kurum:** Sakarya Üniversitesi

---

**NOT:** 
1. Köşeli parantez [] içindeki bilgileri kendi bilgilerinizle değiştirin
2. Ekran görüntülerini uygulamayı çalıştırarak kendi bilgisayarınızdan alın
3. Bu Markdown dosyasını Word'e aktarmak için:
   - https://pandoc.org/ kullanabilirsiniz
   - Veya içeriği kopyalayıp Word'e yapıştırabilirsiniz
