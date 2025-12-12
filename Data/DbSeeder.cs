using FitnessCenterApp.Data;
using FitnessCenterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterApp.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // E?er veri varsa seed yapma
            if (await context.FitnessCenters.AnyAsync())
                return;

            // Fitness Centers
            var fitnessCenters = new List<FitnessCenter>
            {
                new FitnessCenter
                {
                    Name = "Sakarya Fitness Center",
                    Address = "Serdivan, Sakarya",
                    PhoneNumber = "0264 123 45 67",
                    Email = "info@sakaryafitness.com",
                    OpeningTime = new TimeSpan(6, 0, 0),
                    ClosingTime = new TimeSpan(23, 0, 0),
                    Description = "Modern ekipmanlar ve profesyonel antrenörler ile hizmetinizdeyiz.",
                    IsActive = true
                },
                new FitnessCenter
                {
                    Name = "Elite Sports Club",
                    Address = "Adapazar?, Sakarya",
                    PhoneNumber = "0264 987 65 43",
                    Email = "contact@elitesports.com",
                    OpeningTime = new TimeSpan(7, 0, 0),
                    ClosingTime = new TimeSpan(22, 0, 0),
                    Description = "Lüks fitness deneyimi için Elite Sports Club'a bekleriz.",
                    IsActive = true
                }
            };

            await context.FitnessCenters.AddRangeAsync(fitnessCenters);
            await context.SaveChangesAsync();

            // Trainers
            var trainers = new List<Trainer>
            {
                new Trainer
                {
                    FirstName = "Ahmet",
                    LastName = "Y?lmaz",
                    Email = "ahmet.yilmaz@fitness.com",
                    PhoneNumber = "0555 111 22 33",
                    Specialization = "Kuvvet Antrenman? ve Kas Geli?tirme",
                    Bio = "10 y?ll?k deneyime sahip profesyonel fitness antrenörü",
                    ExperienceYears = 10,
                    IsAvailable = true,
                    FitnessCenterId = fitnessCenters[0].Id
                },
                new Trainer
                {
                    FirstName = "Ay?e",
                    LastName = "Demir",
                    Email = "ayse.demir@fitness.com",
                    PhoneNumber = "0555 222 33 44",
                    Specialization = "Yoga ve Pilates",
                    Bio = "Sertifikal? yoga ve pilates e?itmeni",
                    ExperienceYears = 7,
                    IsAvailable = true,
                    FitnessCenterId = fitnessCenters[0].Id
                },
                new Trainer
                {
                    FirstName = "Mehmet",
                    LastName = "Kaya",
                    Email = "mehmet.kaya@fitness.com",
                    PhoneNumber = "0555 333 44 55",
                    Specialization = "Kardiyovasküler Egzersiz ve Kilo Verme",
                    Bio = "Beslenme ve kardiyovasküler egzersiz uzman?",
                    ExperienceYears = 8,
                    IsAvailable = true,
                    FitnessCenterId = fitnessCenters[1].Id
                }
            };

            await context.Trainers.AddRangeAsync(trainers);
            await context.SaveChangesAsync();

            // Services
            var services = new List<Service>
            {
                new Service
                {
                    Name = "Ki?isel Antrenman",
                    Description = "Bire bir ki?isel antrenman seans?",
                    DurationMinutes = 60,
                    Price = 200,
                    ServiceType = "Fitness",
                    IsActive = true,
                    FitnessCenterId = fitnessCenters[0].Id
                },
                new Service
                {
                    Name = "Grup Yoga Dersi",
                    Description = "Grup halinde yoga dersi",
                    DurationMinutes = 90,
                    Price = 100,
                    ServiceType = "Yoga",
                    IsActive = true,
                    FitnessCenterId = fitnessCenters[0].Id
                },
                new Service
                {
                    Name = "Pilates Seans?",
                    Description = "Bire bir pilates antrenman?",
                    DurationMinutes = 60,
                    Price = 150,
                    ServiceType = "Pilates",
                    IsActive = true,
                    FitnessCenterId = fitnessCenters[0].Id
                },
                new Service
                {
                    Name = "Kardio Program?",
                    Description = "Kilo verme odakl? kardiyovasküler program",
                    DurationMinutes = 45,
                    Price = 120,
                    ServiceType = "Kardio",
                    IsActive = true,
                    FitnessCenterId = fitnessCenters[1].Id
                },
                new Service
                {
                    Name = "Beslenme Dan??manl???",
                    Description = "Ki?iselle?tirilmi? beslenme program?",
                    DurationMinutes = 30,
                    Price = 150,
                    ServiceType = "Dan??manl?k",
                    IsActive = true,
                    FitnessCenterId = fitnessCenters[1].Id
                }
            };

            await context.Services.AddRangeAsync(services);
            await context.SaveChangesAsync();

            // Trainer Services
            var trainerServices = new List<TrainerService>
            {
                new TrainerService { TrainerId = trainers[0].Id, ServiceId = services[0].Id },
                new TrainerService { TrainerId = trainers[0].Id, ServiceId = services[3].Id },
                new TrainerService { TrainerId = trainers[1].Id, ServiceId = services[1].Id },
                new TrainerService { TrainerId = trainers[1].Id, ServiceId = services[2].Id },
                new TrainerService { TrainerId = trainers[2].Id, ServiceId = services[3].Id },
                new TrainerService { TrainerId = trainers[2].Id, ServiceId = services[4].Id }
            };

            await context.TrainerServices.AddRangeAsync(trainerServices);
            await context.SaveChangesAsync();

            // Trainer Availabilities
            var availabilities = new List<TrainerAvailability>();
            
            // Her antrenör için Pazartesi-Cuma 09:00-18:00 aras? müsait
            foreach (var trainer in trainers)
            {
                for (int day = 1; day <= 5; day++) // Pazartesi-Cuma
                {
                    availabilities.Add(new TrainerAvailability
                    {
                        TrainerId = trainer.Id,
                        DayOfWeek = (DayOfWeek)day,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(18, 0, 0),
                        IsAvailable = true
                    });
                }
                
                // Cumartesi 10:00-15:00
                availabilities.Add(new TrainerAvailability
                {
                    TrainerId = trainer.Id,
                    DayOfWeek = DayOfWeek.Saturday,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(15, 0, 0),
                    IsAvailable = true
                });
            }

            await context.TrainerAvailabilities.AddRangeAsync(availabilities);
            await context.SaveChangesAsync();
        }
    }
}
