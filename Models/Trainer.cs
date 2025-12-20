// Gerekli kütüphane - Data Annotations (Validasyon attribute'ları)
using System.ComponentModel.DataAnnotations;

namespace FitnessCenterApp.Models
{
    /// <summary>
    /// Antrenör modeli - Fitness merkezinde çalışan antrenörlerin bilgilerini tutar
    /// Her antrenör bir salona bağlıdır ve birden fazla randevuya sahip olabilir
    /// </summary>
    public class Trainer
    {
        // ===================================
        // Primary Key (Birincil Anahtar)
        // ===================================
        /// <summary>
        /// Antrenörün benzersiz kimlik numarası (ID)
        /// Veritabanında otomatik artan değer olarak oluşturulur
        /// </summary>
        public int Id { get; set; }
        
        // ===================================
        // Kişisel Bilgiler
        // ===================================
        
        /// <summary>
        /// Antrenörün adı
        /// </summary>
        [Required(ErrorMessage = "Ad zorunludur")]        // Zorunlu alan - Boş bırakılamaz
        [StringLength(50)]                                // Maksimum 50 karakter uzunluğunda olabilir
        public string FirstName { get; set; } = string.Empty;
        
        /// <summary>
        /// Antrenörün soyadı
        /// </summary>
        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        // ===================================
        // İletişim Bilgileri
        // ===================================
        
        /// <summary>
        /// Antrenörün e-posta adresi
        /// İletişim ve bildirimler için kullanılır
        /// </summary>
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]                                    // Email formatı kontrolü yapar
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Antrenörün telefon numarası
        /// Nullable: Boş bırakılabilir (?)
        /// </summary>
        [Phone]                                           // Telefon numarası formatı kontrolü
        public string? PhoneNumber { get; set; }
        
        // ===================================
        // Mesleki Bilgiler
        // ===================================
        
        /// <summary>
        /// Antrenörün uzmanlık alanı
        /// Örnek: "Fitness, Yoga", "Kişisel Antrenman", "Pilates"
        /// </summary>
        [Required(ErrorMessage = "Uzmanlık alanı zorunludur")]
        [StringLength(200)]                               // Maksimum 200 karakter
        public string Specialization { get; set; } = string.Empty;
        
        /// <summary>
        /// Antrenör hakkında detaylı bilgi (Biyografi)
        /// Özgeçmiş, deneyimler, özel yetenekler vb.
        /// Nullable: Boş bırakılabilir
        /// </summary>
        public string? Bio { get; set; }
        
        /// <summary>
        /// Antrenörün profil fotoğrafı URL'i
        /// Örnek: "/uploads/trainer_123.jpg"
        /// Nullable: Fotoğraf yüklenmemişse null olabilir
        /// </summary>
        public string? ProfileImageUrl { get; set; }
        
        /// <summary>
        /// Antrenörlük deneyim yılı
        /// Kaç yıldır bu işi yaptığını belirtir
        /// </summary>
        [Range(0, 50)]                                    // 0 ile 50 yıl arasında olmalı
        public int ExperienceYears { get; set; }
        
        // ===================================
        // Durum Bilgisi
        // ===================================
        
        /// <summary>
        /// Antrenörün aktif olup olmadığı (Randevu alıp almadığı)
        /// true: Müsait, randevu alabilir
        /// false: Müsait değil, randevu alamaz
        /// Varsayılan değer: true
        /// </summary>
        public bool IsAvailable { get; set; } = true;
        
        // ===================================
        // Foreign Key (Yabancı Anahtar)
        // ===================================
        
        /// <summary>
        /// Antrenörün çalıştığı fitness merkezinin ID'si
        /// FitnessCenter tablosuna referans verir
        /// Her antrenör bir salona bağlıdır
        /// </summary>
        public int FitnessCenterId { get; set; }
        
        // ===================================
        // Navigation Properties (İlişkisel Özellikler)
        // ===================================
        // Navigation Properties: Entity Framework'ün ilişkili verileri otomatik yüklemesi için
        // virtual: Lazy loading (gerektiğinde yükleme) özelliğini aktif eder
        
        /// <summary>
        /// Antrenörün çalıştığı fitness merkezi bilgisi
        /// FitnessCenter nesnesine navigation property
        /// Salonun adı, adresi, çalışma saatleri gibi bilgilere erişim sağlar
        /// </summary>
        public virtual FitnessCenter? FitnessCenter { get; set; }
        
        /// <summary>
        /// Antrenörün müsaitlik saatleri listesi
        /// TrainerAvailability koleksiyonu - Hangi günlerde hangi saatlerde müsait
        /// Örnek: Pazartesi 09:00-17:00, Çarşamba 10:00-18:00
        /// ICollection: Birden fazla kayıt tutabilir
        /// </summary>
        public virtual ICollection<TrainerAvailability> Availabilities { get; set; } = new List<TrainerAvailability>();
        
        /// <summary>
        /// Antrenörün randevu listesi
        /// Appointment koleksiyonu - Geçmiş ve gelecek tüm randevular
        /// Bu liste üzerinden antrenörün yoğunluğu hesaplanabilir
        /// </summary>
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        
        /// <summary>
        /// Antrenörün verebileceği hizmetler listesi
        /// TrainerService koleksiyonu - Hangi hizmetleri verebileceğini belirtir
        /// Örnek: Kişisel Antrenman, Grup Dersi, Diyet Danışmanlığı
        /// Many-to-Many ilişki için ara tablo
        /// </summary>
        public virtual ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
    }
}
