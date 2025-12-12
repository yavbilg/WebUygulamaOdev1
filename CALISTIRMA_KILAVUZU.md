# Fitness Center Uygulamas?n? Çal??t?rma K?lavuzu

## H?zl? Ba?lang?ç

### 1. Gerekli Araçlar
- ? .NET 10.0 SDK yüklü
- ? Visual Studio 2022 veya VS Code
- ? SQL Server LocalDB (Visual Studio ile birlikte gelir)

### 2. Projeyi Çal??t?rma

#### Visual Studio ile:
1. `FitnessCenterApp.csproj` dosyas?na çift t?klay?n
2. Visual Studio aç?lacak
3. `F5` tu?una bas?n veya "Run" butonuna t?klay?n
4. Taray?c? otomatik aç?lacak

#### Komut Sat?r? ile:
```bash
# 1. Proje klasörüne gidin
cd C:\Users\yavuz\source\repos\WebUygulamaOdev1

# 2. Gerekli paketleri yükleyin (ilk kez)
dotnet restore

# 3. Veritaban?n? güncelleyin (ilk kez)
dotnet ef database update

# 4. Uygulamay? çal??t?r?n
dotnet run

# 5. Taray?c?dan aç?n:
# https://localhost:5001
# veya
# http://localhost:5000
```

### 3. ?lk Giri?

#### Admin Hesab?:
- **Email:** ogrencinumarasi@sakarya.edu.tr
- **?ifre:** sau

#### Yeni Üye Kayd?:
1. Ana sayfada "Kay?t Ol" butonuna t?klay?n
2. Formu doldurun
3. Otomatik giri? yap?lacak

### 4. Örnek Senaryolar

#### Senaryo 1: Admin Olarak Salon Eklemek
1. Admin hesab? ile giri? yap?n
2. Üst menüden "Yönetim" ? "Salonlar"
3. "Yeni Salon Ekle" butonuna t?klay?n
4. Formu doldurup kaydedin

#### Senaryo 2: Antrenör Eklemek
1. Admin hesab? ile giri? yap?n
2. "Yönetim" ? "Antrenörler"
3. "Yeni Antrenör Ekle"
4. Bilgileri girin ve kaydedin
5. **Not:** Önce en az bir salon eklemi? olmal?s?n?z

#### Senaryo 3: Hizmet Eklemek
1. Admin hesab? ile giri? yap?n
2. "Yönetim" ? "Hizmetler"
3. "Yeni Hizmet Ekle"
4. Hizmet detaylar?n? girin

#### Senaryo 4: Üye Olarak Randevu Almak
1. Üye hesab? ile giri? yap?n (veya kay?t olun)
2. "Yeni Randevu" butonuna t?klay?n
3. Antrenör seçin
4. Hizmet seçin
5. Tarih seçin
6. Müsait saatlerden birini seçin
7. "Randevu Olu?tur"

#### Senaryo 5: AI Önerisi Almak
1. Üye hesab? ile giri? yap?n
2. "AI Önerisi" menüsüne t?klay?n
3. Boy, kilo, ya? bilgilerinizi girin
4. Hedefinizi seçin (Kilo verme, Kas yapma, vb.)
5. ?ste?e ba?l?: Foto?raf yükleyin
6. "AI Önerisi Al" butonuna t?klay?n
7. Ki?iselle?tirilmi? egzersiz ve diyet plan?n?z? görün

### 5. API Test Etme

#### Postman ile:
```
GET https://localhost:5001/api/trainers
GET https://localhost:5001/api/trainers/1
GET https://localhost:5001/api/trainers?specialization=yoga
GET https://localhost:5001/api/appointments
GET https://localhost:5001/api/appointments/statistics
```

#### Taray?c? ile:
```
https://localhost:5001/api/trainers
https://localhost:5001/api/appointments
```

### 6. Sorun Giderme

#### Port Çak??mas?
E?er "Port zaten kullan?l?yor" hatas? al?rsan?z:
```bash
# appsettings.json veya launchSettings.json dosyas?nda
# port numaras?n? de?i?tirin
```

#### Veritaban? Hatas?
```bash
# Migrations'? s?f?rlay?n
dotnet ef database drop
dotnet ef database update
```

#### Build Hatas?
```bash
# Clean ve rebuild yap?n
dotnet clean
dotnet build
```

#### NuGet Paket Hatas?
```bash
# Paketleri temizleyip yeniden yükleyin
dotnet nuget locals all --clear
dotnet restore
```

### 7. Veritaban?n? S?f?rlama

E?er veritaban?n? ba?tan olu?turmak isterseniz:

```bash
# 1. Veritaban?n? sil
dotnet ef database drop

# 2. Tekrar olu?tur
dotnet ef database update

# 3. Uygulamay? çal??t?r (otomatik seed data eklenecek)
dotnet run
```

### 8. Örnek Veri (Seed Data)

Uygulama ilk çal??t?r?ld???nda otomatik olarak ?u veriler eklenir:

- ? 2 Fitness Salon
- ? 3 Antrenör
- ? 5 Hizmet
- ? Antrenör müsaitlik saatleri
- ? Admin kullan?c?s?

### 9. Önemli URL'ler

```
Ana Sayfa:          https://localhost:5001/
Kay?t Ol:           https://localhost:5001/Account/Register
Giri? Yap:          https://localhost:5001/Account/Login
Admin Dashboard:    https://localhost:5001/Admin
Randevular?m:       https://localhost:5001/Appointments
Yeni Randevu:       https://localhost:5001/Appointments/Create
AI Önerisi:         https://localhost:5001/AI/Recommendation
API Trainers:       https://localhost:5001/api/trainers
API Appointments:   https://localhost:5001/api/appointments
```

### 10. Performans ?puçlar?

- ?lk aç?l?? biraz yava? olabilir (seed data ekleniyor)
- Veritaban? LocalDB kullan?yor, SQL Server kadar h?zl? olmayabilir
- Development modunda çal??t?r?ld???nda daha fazla log üretilir

### 11. Güvenlik Notlar?

- ?? Admin ?ifresi çok basit (sadece demo için)
- ?? Production'da güçlü ?ifreler kullan?n
- ?? appsettings.json'daki API key'leri koruyun
- ?? HTTPS kullan?m? zorunlu

### 12. Geli?tirme ?puçlar?

#### Hot Reload (Canl? Yeniden Yükleme)
```bash
dotnet watch run
```
Bu komutla kod de?i?ikliklerinde otomatik yeniden ba?latma aktif olur.

#### Migration Ekleme
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

#### Migration Geri Alma
```bash
dotnet ef migrations remove
```

### 13. Test Kullan?c?lar?

Sisteme kay?t olmak yerine test için kullanabilirsiniz:

**Admin:**
- Email: ogrencinumarasi@sakarya.edu.tr
- ?ifre: sau
- Rol: Admin
- Yetkiler: Tüm yönetim i?lemleri

**Demo Member (Kendiniz olu?turabilirsiniz):**
- Email: test@test.com
- ?ifre: Test123! (kay?t s?ras?nda)
- Rol: Member
- Yetkiler: Randevu alma, AI önerisi alma

### 14. Mobil Görünüm

Uygulama responsive tasar?ma sahip:
- ?? Mobil cihazlarda test edin
- ?? Tablet görünümü
- ??? Desktop görünümü

Chrome DevTools ile test:
- F12 tu?u
- "Toggle device toolbar" (Ctrl+Shift+M)
- Farkl? cihazlar? seçin

### 15. Loglama

Konsol ç?kt?lar?n? izleyin:
```bash
dotnet run

# ?u gibi loglar göreceksiniz:
info: Microsoft.EntityFrameworkCore.Database.Command
info: Microsoft.AspNetCore.Hosting.Diagnostics
```

### 16. Yayg?n Hatalar ve Çözümleri

**"Cannot connect to SQL Server"**
? SQL Server LocalDB yüklü mü kontrol edin
? Connection string do?ru mu?

**"Login failed for user"**
? Veritaban?n? drop edip yeniden olu?turun

**"Port 5001 already in use"**
? Ba?ka bir uygulama 5001 portunu kullan?yor
? Portu de?i?tirin veya di?er uygulamay? kapat?n

**"Migration not found"**
? `dotnet ef migrations add InitialCreate` çal??t?r?n

**"Seed data not loading"**
? Veritaban?n? temizleyin ve yeniden çal??t?r?n

### 17. Ba?ar?l? Çal??ma Kontrolü

Uygulama ba?ar?yla çal???yorsa:

? Ana sayfa aç?l?yor
? Kay?t olunabiliyor
? Giri? yap?labiliyor
? Admin paneli eri?ilebilir (admin ile)
? Randevu olu?turulabiliyor
? AI önerisi al?nabiliyor
? API endpoint'leri çal???yor

### 18. Ekstra: Docker ile Çal??t?rma (?leri Düzey)

E?er Docker kullanmak isterseniz:

```dockerfile
# Dockerfile (olu?turman?z gerekir)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["FitnessCenterApp.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitnessCenterApp.dll"]
```

```bash
docker build -t fitnesscenter .
docker run -p 8080:80 fitnesscenter
```

---

## Yard?m ve Destek

Sorun ya?arsan?z:
1. Hata mesaj?n? dikkatlice okuyun
2. Console loglar?n? kontrol edin
3. README.md dosyas?na bak?n
4. Google'da hata mesaj?n? arat?n
5. Stack Overflow'da aray?n

## Ba?ar?lar! ??

Projeniz haz?r! Test etmeye ba?layabilirsiniz.
