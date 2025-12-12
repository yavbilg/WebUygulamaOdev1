using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApp.Models;

namespace FitnessCenterApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FitnessCenter> FitnessCenters { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<TrainerService> TrainerServices { get; set; }
        public DbSet<TrainerAvailability> TrainerAvailabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MembershipPlan> MembershipPlans { get; set; }
        public DbSet<AIRecommendation> AIRecommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FitnessCenter
            modelBuilder.Entity<FitnessCenter>()
                .HasMany(fc => fc.Trainers)
                .WithOne(t => t.FitnessCenter)
                .HasForeignKey(t => t.FitnessCenterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FitnessCenter>()
                .HasMany(fc => fc.Services)
                .WithOne(s => s.FitnessCenter)
                .HasForeignKey(s => s.FitnessCenterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Trainer
            modelBuilder.Entity<Trainer>()
                .HasMany(t => t.Availabilities)
                .WithOne(a => a.Trainer)
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trainer>()
                .HasMany(t => t.Appointments)
                .WithOne(a => a.Trainer)
                .HasForeignKey(a => a.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);

            // TrainerService - Many-to-Many
            modelBuilder.Entity<TrainerService>()
                .HasOne(ts => ts.Trainer)
                .WithMany(t => t.TrainerServices)
                .HasForeignKey(ts => ts.TrainerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TrainerService>()
                .HasOne(ts => ts.Service)
                .WithMany(s => s.TrainerServices)
                .HasForeignKey(ts => ts.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // MembershipPlan
            modelBuilder.Entity<MembershipPlan>()
                .HasOne(mp => mp.User)
                .WithMany(u => u.MembershipPlans)
                .HasForeignKey(mp => mp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AIRecommendation
            modelBuilder.Entity<AIRecommendation>()
                .HasOne(ai => ai.User)
                .WithMany()
                .HasForeignKey(ai => ai.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal precision
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MembershipPlan>()
                .Property(mp => mp.Price)
                .HasPrecision(18, 2);
        }
    }
}
