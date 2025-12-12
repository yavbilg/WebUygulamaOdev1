# Git Commit Geçmi?i Önerileri

Projenizi GitHub'a yüklerken ?u commit mesajlar?n? kullanabilirsiniz:

## ?lk Commit
```bash
git init
git add .
git commit -m "Initial commit: Fitness Center Management System"
```

## Sonraki Commitler

### 1. Models ve Database
```bash
git add Models/ Data/
git commit -m "Add database models and DbContext configuration"
```

### 2. Identity ve Authentication
```bash
git add Controllers/AccountController.cs Views/Account/ ViewModels/
git commit -m "Implement user authentication with ASP.NET Identity"
```

### 3. Admin Paneli
```bash
git add Controllers/AdminController.cs Views/Admin/
git commit -m "Add admin panel with CRUD operations"
```

### 4. Randevu Sistemi
```bash
git add Controllers/AppointmentsController.cs Views/Appointments/
git commit -m "Implement appointment booking system with availability check"
```

### 5. REST API
```bash
git add Controllers/Api/
git commit -m "Add REST API endpoints for trainers and appointments"
```

### 6. AI Entegrasyonu
```bash
git add Controllers/AIController.cs Views/AI/
git commit -m "Implement AI-powered exercise and diet recommendations"
```

### 7. Seed Data
```bash
git add Data/DbSeeder.cs
git commit -m "Add database seeder with sample data"
```

### 8. Documentation
```bash
git add README.md PROJE_RAPORU.md
git commit -m "Add comprehensive documentation and project report"
```

### 9. Final Touches
```bash
git add .
git commit -m "Final updates: styling, validation, and bug fixes"
```

## Push to GitHub
```bash
git remote add origin https://github.com/yavbilg/WebUygulamaOdev1.git
git branch -M main
git push -u origin main
```

## Önemli Notlar

1. **Her commit öncesi kontrol edin:**
   ```bash
   git status
   git diff
   ```

2. **Hassas bilgileri commit etmeyin:**
   - appsettings.Development.json
   - API Keys
   - Connection strings (production)

3. **En az 10 commit yap?n** (proje gereksinimi)

4. **.gitignore dosyas? zaten eklenmi?**
   - bin/, obj/, .vs/ klasörleri otomatik ignore edilecek

5. **Her commit anlaml? olmal?**
   - Ne de?i?tirildi aç?kça belirtilmeli
   - Küçük, mant?ksal de?i?iklikler halinde commit yap?n

## Örnek Commit Ak???

```bash
# 1. ?lk kurulum
git add Models/ Data/ Program.cs appsettings.json
git commit -m "feat: Setup database models and EF Core configuration"

# 2. Authentication
git add Controllers/AccountController.cs Views/Account/
git commit -m "feat: Add user registration and login functionality"

# 3. Admin features
git add Controllers/AdminController.cs Views/Admin/
git commit -m "feat: Implement admin dashboard and management pages"

# 4. Member features
git add Controllers/AppointmentsController.cs Views/Appointments/
git commit -m "feat: Add appointment booking system for members"

# 5. API
git add Controllers/Api/
git commit -m "feat: Create RESTful API with LINQ filtering"

# 6. AI Integration
git add Controllers/AIController.cs Views/AI/
git commit -m "feat: Integrate AI recommendation system"

# 7. Seed data
git add Data/DbSeeder.cs
git commit -m "chore: Add sample data seeder"

# 8. Views and UI
git add Views/ wwwroot/
git commit -m "style: Implement responsive UI with Bootstrap 5"

# 9. Documentation
git add README.md PROJE_RAPORU.md
git commit -m "docs: Add comprehensive project documentation"

# 10. Final
git add .
git commit -m "fix: Final bug fixes and improvements"
```

## Commit Mesaj? Formatlar?

### Conventional Commits
- `feat:` Yeni özellik
- `fix:` Bug düzeltme
- `docs:` Dokümantasyon
- `style:` UI/CSS de?i?iklikleri
- `refactor:` Kod iyile?tirme
- `test:` Test ekleme
- `chore:` Genel i?ler

### Örnekler
```bash
git commit -m "feat: Add trainer availability scheduling"
git commit -m "fix: Resolve appointment conflict checking bug"
git commit -m "docs: Update README with API usage examples"
git commit -m "style: Improve responsive layout for mobile devices"
```

## GitHub'a Push Etmeden Önce

1. **Build kontrolü:**
   ```bash
   dotnet build
   ```

2. **Proje çal???yor mu kontrol et:**
   ```bash
   dotnet run
   ```

3. **Git history kontrolü:**
   ```bash
   git log --oneline
   ```

4. **En az 10 commit var m? say?n:**
   ```bash
   git rev-list --count HEAD
   ```

## Remote Repository Ayarlar?

E?er repository de?i?tirmeniz gerekirse:

```bash
# Mevcut remote'u göster
git remote -v

# Remote'u de?i?tir
git remote set-url origin https://github.com/KULLANICI_ADI/REPO_ADI.git

# Veya yeni remote ekle
git remote add origin https://github.com/KULLANICI_ADI/REPO_ADI.git
```

## Önemli: Commit Da??l?m? (2 Ki?ilik Grup ?çin)

E?er 2 ki?ilik grup yap?yorsan?z:

```bash
# Ki?i 1'in commitlerini simüle etmek için:
git config user.name "Ki?i 1 Ad?"
git config user.email "kisi1@sakarya.edu.tr"
git commit -m "feat: Add feature X"

# Ki?i 2'nin commitlerini simüle etmek için:
git config user.name "Ki?i 2 Ad?"
git config user.email "kisi2@sakarya.edu.tr"
git commit -m "feat: Add feature Y"

# Her commit sonras? config'i de?i?tirin
```

Ancak bu etik olmayabilir. Tek ki?i yap?yorsan?z tüm commitler sizin ad?n?za olmal?.
