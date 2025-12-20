// Gerekli kütüphane - Data Annotations (Validasyon attribute'ları)
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    /// <summary>
    /// Randevu durumlarını tanımlayan Enum
    /// Enum: Sabit değerlerin listesi, tip güvenli bir şekilde durum yönetimi sağlar
    /// </summary>
    public enum AppointmentStatus
    {
        Pending,    // Beklemede - Randevu oluşturuldu, antrenör onayı bekleniyor
        Confirmed,  // Onaylandı - Antrenör randevuyu onayladı
        Cancelled,  // İptal edildi - Üye veya antrenör tarafından iptal edildi
        Completed   // Tamamlandı - Randevu gerçekleşti
    }

    /// <summary>
    /// Randevu modeli - Üyeler ve antrenörler arasındaki randevu kayıtlarını tutar
    /// Her randevu bir üye, bir antrenör, bir hizmet ve bir zaman dilimine sahiptir
    /// </summary>
    public class Appointment
    {
        // ===================================
        // Primary Key (Birincil Anahtar)
        // ===================================
        /// <summary>
        /// Randevunun benzersiz kimlik numarası (ID)
        /// Veritabanında otomatik artan değer olarak oluşturulur
        /// </summary>
        public int Id { get; set; }
        
        // ===================================
        // Foreign Keys (Yabancı Anahtarlar)
        // ===================================
        // Foreign Key'ler: Diğer tablolara referans veren alanlar
        
        /// <summary>
        /// Randevuyu alan üyenin kullanıcı ID'si
        /// ApplicationUser tablosuna referans verir
        /// </summary>
        [Required(ErrorMessage = "Üye seçimi zorunludur")]
        public string UserId { get; set; } = string.Empty;
        
        /// <summary>
        /// Randevuyu verecek antrenörün ID'si
        /// Trainer tablosuna referans verir
        /// </summary>
        [Required(ErrorMessage = "Antrenör seçimi zorunludur")]
        public int TrainerId { get; set; }
        
        /// <summary>
        /// Randevuda verilecek hizmetin ID'si
        /// Service tablosuna referans verir (Örn: Kişisel Antrenman, Yoga)
        /// </summary>
        [Required(ErrorMessage = "Hizmet seçimi zorunludur")]
        public int ServiceId { get; set; }
        
        // ===================================
        // Randevu Tarih ve Saat Bilgileri
        // ===================================
        
        /// <summary>
        /// Randevunun yapılacağı tarih
        /// DateTime: Tarih ve saat bilgisini tutar, burada sadece tarih kısmı kullanılır
        /// </summary>
        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        public DateTime AppointmentDate { get; set; }
        
        /// <summary>
        /// Randevunun başlangıç saati
        /// TimeSpan: Saat bilgisini tutar (Örn: 14:30)
        /// </summary>
        [Required(ErrorMessage = "Başlangıç saati zorunludur")]
        public TimeSpan StartTime { get; set; }
        
        /// <summary>
        /// Randevunun bitiş saati
        /// Hizmetin süresine göre otomatik hesaplanır
        /// Örnek: Başlangıç 14:00, Hizmet süresi 60 dk => Bitiş 15:00
        /// </summary>
        [Required(ErrorMessage = "Bitiş saati zorunludur")]
        public TimeSpan EndTime { get; set; }
        
        // ===================================
        // Randevu Durumu ve Notlar
        // ===================================
        
        /// <summary>
        /// Randevunun mevcut durumu (Beklemede, Onaylı, İptal, Tamamlandı)
        /// Varsayılan değer: Pending (Beklemede)
        /// </summary>
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        
        /// <summary>
        /// Randevu notları - Üyenin özel istekleri veya açıklamaları
        /// Nullable: Boş bırakılabilir (?)
        /// </summary>
        public string? Notes { get; set; }
        
        /// <summary>
        /// Randevunun oluşturulma tarihi ve saati
        /// Kayıt anında otomatik olarak şu anki zaman atanır
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        // ===================================
        // Navigation Properties (İlişkisel Özellikler)
        // ===================================
        // Navigation Properties: Entity Framework'ün ilişkili verileri otomatik yüklemesi için kullanılır
        // virtual: Lazy loading (gerektiğinde yükleme) özelliğini aktif eder
        
        /// <summary>
        /// Randevuyu alan üye bilgisi
        /// ApplicationUser nesnesine navigation property
        /// Include() metodu ile yüklenebilir
        /// </summary>
        public virtual ApplicationUser? User { get; set; }
        
        /// <summary>
        /// Randevuyu veren antrenör bilgisi
        /// Trainer nesnesine navigation property
        /// Antrenörün adı, soyadı, uzmanlık alanı gibi bilgilere erişim sağlar
        /// </summary>
        public virtual Trainer? Trainer { get; set; }
        
        /// <summary>
        /// Randevuda verilecek hizmet bilgisi
        /// Service nesnesine navigation property
        /// Hizmetin adı, süresi, fiyatı gibi bilgilere erişim sağlar
        /// </summary>
        public virtual Service? Service { get; set; }
    }
}
