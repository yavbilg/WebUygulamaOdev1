# Visual Studio vs dotnet run Karşılaştırma Rehberi

## 🔍 Farklılıkları Test Etmek İçin

### 1. Her İki Yöntemle Çalıştırın ve Karşılaştırın

#### A) dotnet run ile Çalıştırma
```powershell
# Terminal/PowerShell
dotnet run

# Çıktıda göreceksiniz:
Now listening on: http://localhost:5000
Now listening on: https://localhost:5001
```

**Tarayıcıda açın:**
- http://localhost:5000
- https://localhost:5001

#### B) Visual Studio ile Çalıştırma
1. Projeyi VS'de açın
2. F5'e basın (Debug modda)
3. Veya Ctrl+F5 (Release modda)

**Tarayıcıda açılacak:**
- Otomatik açılan URL'yi not edin

---

## 🎨 Görsel Farklılıklar Kontrol Listesi

### Ana Sayfada Kontrol Edilecekler:

#### ✅ Doğru Görünüm (CSS Yüklendiyse):
- [ ] Navbar **MOR-MAVİ gradient** arka planlı
- [ ] Büyük başlık (Hero Section) **gradient renkli**
- [ ] Butonlar **yuvarlatılmış köşeli** ve **renkli**
- [ ] Kartlar **gölgeli** ve **hover'da büyüyor**
- [ ] İkonlar (Font Awesome) **görünüyor**
- [ ] Animasyonlar **çalışıyor** (fade-in, slide-in)

#### ❌ Yanlış Görünüm (CSS Yüklenmemişse):
- [ ] Navbar **düz mavi** (gradient yok)
- [ ] Başlıklar **siyah metin**, gradient yok
- [ ] Butonlar **kare köşeli**, basit
- [ ] Kartlar **gölgesiz**, düz beyaz
- [ ] İkonlar **kutucuk** olarak görünüyor (□)
- [ ] Hiç animasyon yok

---

## 🔧 Debug için F12 Kontrolleri

### Her iki yöntemde de:

1. **F12** tuşuna basın
2. **Network** sekmesine gidin
3. Sayfayı yenileyin (F5)
4. Aşağıdaki dosyaları arayın:

| Dosya | Status | Size | dotnet run | VS Debug |
|-------|--------|------|------------|----------|
| bootstrap.min.css | 200 | ~190KB | ✅ | ? |
| site.css | 200 | ~9KB | ✅ | ? |
| jquery.min.js | 200 | ~85KB | ✅ | ? |
| bootstrap.bundle.min.js | 200 | ~59KB | ✅ | ? |

**Eğer 404 varsa:** CSS/JS yüklenmemiş
**Eğer 200 ise:** Dosya başarıyla yüklendi

---

## 📸 Console Kontrolleri

### Console'da (F12 → Console) kontrol edin:

#### Başarılı Yüklenme:
```javascript
✓ jQuery loaded: 3.x.x
✓ Bootstrap loaded
✓ Bootstrap CSS loaded
```

#### Başarısız Yüklenme:
```javascript
✗ Failed to load resource: net::ERR_FILE_NOT_FOUND
✗ Uncaught ReferenceError: jQuery is not defined
✗ Uncaught ReferenceError: bootstrap is not defined
```

---

## 🎯 Test Sayfası Kullanımı

Her iki yöntemle şu URL'i açın:

```
http://localhost:5000/Home/Test
```

Bu sayfa size şunları gösterecek:
- Environment bilgileri
- Hangi dosyalar yüklendi
- Hangi dosyalar yüklenemedi
- Otomatik test sonuçları

---

## 🔎 Olası Fark Nedenleri

### 1. **Farklı Environment**
- **dotnet run:** Development
- **VS Debug:** Development
- **VS Release (Ctrl+F5):** Production

**Kontrol:**
```csharp
// Program.cs'de
if (!app.Environment.IsDevelopment())
{
    // Production ayarları
}
else
{
    // Development ayarları
}
```

### 2. **Farklı Portlar**
- **dotnet run:** launchSettings.json'daki portlar (5000/5001)
- **VS:** IIS Express portları kullanabilir

**Kontrol:**
- Properties/launchSettings.json dosyasına bakın
- IIS Express profili var mı?

### 3. **Cache Farklılıkları**
- Tarayıcı farklı portları farklı cache'ler
- http://localhost:5000 ≠ http://localhost:5001
- http://localhost:5000 ≠ https://localhost:5001

**Çözüm:**
- Incognito/InPrivate modda test edin
- Hard Refresh: Ctrl + Shift + R

### 4. **Build Konfigürasyonu**
- **Debug:** Optimizasyon yok, tüm dosyalar
- **Release:** Minified, optimized

---

## 📋 Karşılaştırma Formu

Aşağıdaki tabloyu doldurun:

| Özellik | dotnet run | VS Debug | VS Release |
|---------|------------|----------|------------|
| Port | 5000/5001 | ? | ? |
| Gradient navbar | ✅ | ? | ? |
| Gölgeli kartlar | ✅ | ? | ? |
| Animasyonlar | ✅ | ? | ? |
| Font Awesome | ✅ | ? | ? |
| site.css yüklü | ✅ | ? | ? |
| Bootstrap yüklü | ✅ | ? | ? |

---

## 🛠️ Hızlı Düzeltme Adımları

### Eğer VS'de CSS yüklenmiyorsa:

1. **Çalışan tüm instance'ları durdurun**
   ```powershell
   Get-Process -Name "FitnessCenterApp","iisexpress" | Stop-Process -Force
   ```

2. **bin ve obj klasörlerini silin**
   ```powershell
   Remove-Item -Path "bin","obj" -Recurse -Force
   ```

3. **Clean ve Rebuild**
   - Visual Studio: Build → Clean Solution
   - Sonra: Build → Rebuild Solution

4. **Tarayıcı cache'ini temizle**
   - Ctrl + Shift + Delete
   - Veya Incognito modda test et

5. **Yeniden çalıştır**

---

## 📞 Sonuçları Bildirin

Lütfen şunları söyleyin:
1. **dotnet run** ile açıldığında CSS nasıl görünüyor?
2. **VS'den** açıldığında CSS nasıl görünüyor?
3. **F12 → Network** sekmesinde hangi dosyalar 404?
4. **Console'da** hangi hatalar var?
5. **Ekran görüntüsü** paylaşabilir misiniz?

Bu bilgilerle tam olarak sorunu çözebiliriz! 🔧
