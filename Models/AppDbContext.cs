﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using System;
using Transport__system_prototype.Models;
namespace Transport_system_prototype.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.FromStation)
                .WithMany()
                .HasForeignKey(t => t.FromStationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.TOStation)
                .WithMany()
                .HasForeignKey(t => t.TOStationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.vehicle)
                .WithMany()
                .HasForeignKey(t => t.vehicleId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Booking>()
                .HasKey(b => b.Id);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Trip)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TripId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Station>().HasData(
                new Station { Id = 1, Name = "Alexandria", Location = "Alexandria, Egypt", ImgURL = "uploads/Alex.jpeg" },
                new Station { Id = 2, Name = "Dahab", Location = "Dahab, Egypt", ImgURL = "uploads/Dahab.jpeg" },
                new Station { Id = 3, Name = "Hurghada", Location = "Hurghada, Egypt", ImgURL = "uploads/hurghada.jpeg" },
                new Station { Id = 4, Name = "Marsa Alam", Location = "Marsa Alam, Egypt", ImgURL = "uploads/Marsa-Alam.jpeg" },
                new Station { Id = 5, Name = "Nuweiba", Location = "Nuweiba, Egypt", ImgURL = "uploads/Nuweiba.jpeg" }
            );

            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Id = 1, Type = "Train", Name = "Luxury", TV = true, NumberOfSeats = 150, AirConditioning = true, WiFi = true, Drinks = true, Snacks = true, ImgURL = "~/uploads/Train1.jpeg" },
                new Vehicle { Id = 2, Type = "Train", Name = "Standard", TV = false, NumberOfSeats = 200, AirConditioning = true, WiFi = false, Drinks = false, Snacks = false, ImgURL = "~/uploads/Train2.jpeg" },
                new Vehicle { Id = 3, Type = "Bus", Name = "Comfort", TV = true, NumberOfSeats = 30, AirConditioning = true, WiFi = false, Drinks = true, Snacks = true, ImgURL = "~/uploads/bus3.jpeg" },
                new Vehicle { Id = 4, Type = "Bus", Name = "Premium", TV = true, NumberOfSeats = 55, AirConditioning = true, WiFi = true, Drinks = true, Snacks = false, ImgURL = "~/uploads/bus4.jpeg" }
            );

            modelBuilder.Entity<Trip>().HasData(
               new Trip { Id = 1, vehicleId = 1, FromStationId = 1, Price = 300, NumberOfSeats = 10, AvailableSeats = 10, TOStationId = 3, TripDate = new DateTime(2025,  8, 14, 10, 00, 00) }, 
                new Trip { Id = 2, vehicleId = 2, FromStationId = 3, Price = 300, NumberOfSeats = 50, AvailableSeats = 50, TOStationId = 5, TripDate = new DateTime(2025, 8, 15, 12, 30, 00) },
                new Trip { Id = 3, vehicleId = 3, FromStationId = 2, Price = 300, NumberOfSeats = 30, AvailableSeats = 30, TOStationId = 4, TripDate = new DateTime(2025, 8, 16, 14, 45, 00) },
                new Trip { Id = 4, vehicleId = 4, FromStationId = 5, Price = 300, NumberOfSeats = 20, AvailableSeats = 20, TOStationId = 1, TripDate = new DateTime(2025, 8, 17, 16, 00, 00) } 
            );
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<Vehicle> vehicles { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
