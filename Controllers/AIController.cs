using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using FitnessCenterApp.ViewModels;
using System.Text.Json;
using System.Text;

namespace FitnessCenterApp.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public AIController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Recommendation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recommendation(AIRecommendationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                string? imageUrl = null;

                // Save uploaded image if exists
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    imageUrl = "/uploads/" + uniqueFileName;
                }

                // Generate AI recommendation (mock implementation)
                var recommendation = GenerateAIRecommendation(model);

                var aiRecommendation = new AIRecommendation
                {
                    UserId = user!.Id,
                    BodyType = model.BodyType,
                    Height = model.Height,
                    Weight = model.Weight,
                    Age = model.Age,
                    Goal = model.Goal,
                    ImageUrl = imageUrl,
                    Recommendation = recommendation.Recommendation,
                    ExercisePlan = recommendation.ExercisePlan,
                    DietPlan = recommendation.DietPlan,
                    CreatedDate = DateTime.Now
                };

                _context.AIRecommendations.Add(aiRecommendation);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(RecommendationResult), new { id = aiRecommendation.Id });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RecommendationResult(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var recommendation = await _context.AIRecommendations
                .Where(r => r.Id == id && r.UserId == user!.Id)
                .FirstOrDefaultAsync();

            if (recommendation == null)
                return NotFound();

            return View(recommendation);
        }

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            var recommendations = await _context.AIRecommendations
                .Where(r => r.UserId == user!.Id)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return View(recommendations);
        }

        private (string Recommendation, string ExercisePlan, string DietPlan) GenerateAIRecommendation(AIRecommendationViewModel model)
        {
            // Mock AI recommendation - In production, this would call OpenAI API or similar
            var recommendation = new StringBuilder();
            var exercisePlan = new StringBuilder();
            var dietPlan = new StringBuilder();

            // Calculate BMI if height and weight are provided
            if (model.Height.HasValue && model.Weight.HasValue)
            {
                var heightInMeters = model.Height.Value / 100;
                var bmi = model.Weight.Value / (heightInMeters * heightInMeters);

                recommendation.AppendLine($"📊 Vücut Kitle İndeksiniz (BMI): {bmi:F2}");
                
                if (bmi < 18.5)
                    recommendation.AppendLine("Kilonuz normal değerin altında. Kilo almanız önerilir.");
                else if (bmi < 25)
                    recommendation.AppendLine("Kilonuz normal aralıkta. Bu kilonuzu koruyun.");
                else if (bmi < 30)
                    recommendation.AppendLine("Fazla kiloluysunuz. Sağlıklı bir kilo verme programı önerilir.");
                else
                    recommendation.AppendLine("Obezite riski var. Doktor kontrolünde kilo verme programı önerilir.");
            }

            recommendation.AppendLine();
            recommendation.AppendLine($"🎯 Hedefiniz: {model.Goal ?? "Genel Fitness"}");
            recommendation.AppendLine();

            // Generate exercise plan based on goal
            exercisePlan.AppendLine("💪 12 Haftalık Egzersiz Programı:");
            exercisePlan.AppendLine();

            switch (model.Goal?.ToLower())
            {
                case "kilo verme":
                case "kilo vermek":
                    exercisePlan.AppendLine("**Hafta 1-4: Temel Kardiyovasküler Egzersizler**");
                    exercisePlan.AppendLine("• Pazartesi: 30 dk koşu bandı (orta tempo)");
                    exercisePlan.AppendLine("• Çarşamba: 45 dk bisiklet");
                    exercisePlan.AppendLine("• Cuma: 30 dk HIIT antrenmanı");
                    exercisePlan.AppendLine("• Cumartesi: 60 dk yürüyüş");
                    exercisePlan.AppendLine();
                    exercisePlan.AppendLine("**Hafta 5-8: Artırılmış Yoğunluk**");
                    exercisePlan.AppendLine("• Pazartesi: 40 dk koşu (artan tempo)");
                    exercisePlan.AppendLine("• Salı: Kuvvet antrenmanı (full body)");
                    exercisePlan.AppendLine("• Perşembe: 50 dk bisiklet + 15 dk core");
                    exercisePlan.AppendLine("• Cumartesi: 45 dk HIIT + 30 dk yürüyüş");
                    break;

                case "kas geliştirme":
                case "kas yapmak":
                    exercisePlan.AppendLine("**Hafta 1-4: Temel Kuvvet Antrenmanı**");
                    exercisePlan.AppendLine("• Pazartesi: Göğüs + Triseps (Bench press, dips, push-ups)");
                    exercisePlan.AppendLine("• Çarşamba: Sırt + Biseps (Pull-ups, rows, curls)");
                    exercisePlan.AppendLine("• Cuma: Bacak + Omuz (Squat, deadlift, shoulder press)");
                    exercisePlan.AppendLine();
                    exercisePlan.AppendLine("**Hafta 5-8: Hipertrofi Programı**");
                    exercisePlan.AppendLine("• 4 set x 8-12 tekrar");
                    exercisePlan.AppendLine("• Ağırlık artışı: %5-10");
                    exercisePlan.AppendLine("• Dinlenme: 60-90 saniye");
                    break;

                default:
                    exercisePlan.AppendLine("**Hafta 1-4: Dengeli Fitness Programı**");
                    exercisePlan.AppendLine("• Pazartesi: 30 dk kardio + 20 dk kuvvet");
                    exercisePlan.AppendLine("• Çarşamba: Yoga/Pilates (60 dk)");
                    exercisePlan.AppendLine("• Cuma: Full body kuvvet antrenmanı");
                    exercisePlan.AppendLine("• Cumartesi: Aktif dinlenme (yürüyüş, yüzme)");
                    break;
            }

            // Generate diet plan
            dietPlan.AppendLine("🍽️ Beslenme Önerileri:");
            dietPlan.AppendLine();

            if (model.Weight.HasValue)
            {
                var dailyCalories = model.Goal?.ToLower().Contains("kilo verme") == true
                    ? (int)(model.Weight.Value * 24)
                    : (int)(model.Weight.Value * 30);

                dietPlan.AppendLine($"**Günlük Kalori İhtiyacı: ~{dailyCalories} kcal**");
                dietPlan.AppendLine();
            }

            dietPlan.AppendLine("**Sabah Kahvaltısı (07:00-08:00):**");
            dietPlan.AppendLine("• 2 yumurta omlet");
            dietPlan.AppendLine("• 2 dilim tam tahıllı ekmek");
            dietPlan.AppendLine("• Avokado veya zeytinyağı");
            dietPlan.AppendLine("• Mevsim meyveleri");
            dietPlan.AppendLine();

            dietPlan.AppendLine("**Ara Öğün (10:30):**");
            dietPlan.AppendLine("• 1 avuç çiğ badem");
            dietPlan.AppendLine("• 1 muz");
            dietPlan.AppendLine();

            dietPlan.AppendLine("**Öğle Yemeği (12:30-13:30):**");
            dietPlan.AppendLine("• Izgara tavuk/balık (150-200g)");
            dietPlan.AppendLine("• Bol yeşil salata");
            dietPlan.AppendLine("• Bulgur pilavı veya kinoa");
            dietPlan.AppendLine("• Yoğurt");
            dietPlan.AppendLine();

            dietPlan.AppendLine("**Ara Öğün (16:00):**");
            dietPlan.AppendLine("• Protein shake veya ayran");
            dietPlan.AppendLine("• Tam tahıllı kraker");
            dietPlan.AppendLine();

            dietPlan.AppendLine("**Akşam Yemeği (19:00-20:00):**");
            dietPlan.AppendLine("• Izgara et/tavuk/balık");
            dietPlan.AppendLine("• Buharda pişmiş sebzeler");
            dietPlan.AppendLine("• Salata");
            dietPlan.AppendLine();

            dietPlan.AppendLine("**Su Tüketimi:** Günde en az 2-3 litre su için");
            dietPlan.AppendLine("**Not:** Antrenman öncesi ve sonrası beslenmesini ihmal etmeyin!");

            return (recommendation.ToString(), exercisePlan.ToString(), dietPlan.ToString());
        }

        // This method would be used for actual OpenAI integration
        private async Task<string> CallOpenAIAPI(string prompt)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "API Key not configured";
            }

            // OpenAI API implementation would go here
            // This is a placeholder for the actual implementation
            return "AI response would be generated here";
        }
    }
}
