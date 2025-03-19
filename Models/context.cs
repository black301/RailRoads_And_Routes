using Microsoft.EntityFrameworkCore;

namespace Bus_system_prototype.Models
{
    public class context : DbContext
    {
        public context() : base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=MY-BOY;Initial Catalog=SystemPrototype;Integrated Security=True;TrustServerCertificate=True");
            
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.FromStation)
                .WithMany()
                .HasForeignKey(t => t.FromStationId)
                .OnDelete(DeleteBehavior.NoAction); // 🚀 Fixes cascade delete issue

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Bus)
                .WithMany()
                .HasForeignKey(t => t.BusId)
                .OnDelete(DeleteBehavior.NoAction); // Optional (if needed)

            modelBuilder.Entity<Station>().HasData(
                new Station { Id = 1, Name = "Alexandria", Location = "Alexandria, Egypt", ImgURL = "~/uploads/Alex.jpeg" },
                new Station { Id = 2, Name = "Dahab", Location = "Dahab, Egypt", ImgURL = "~/uploads/Dahab.jpeg" },
                new Station { Id = 3, Name = "Hurghada", Location = "Hurghada, Egypt", ImgURL = "~/uploads/hurghada.jpeg" },
                new Station { Id = 4, Name = "Marsa Alam", Location = "Marsa Alam, Egypt", ImgURL = "~/uploads/Marsa-Alam.jpeg" },
                new Station { Id = 5, Name = "Nuweiba", Location = "Nuweiba, Egypt", ImgURL = "~/uploads/Nuweiba.jpeg" }
            );

            modelBuilder.Entity<Bus>().HasData(
                new Bus { Id = 1, Type = "Luxury", TV = true, NumberOfSeats =    10, AirConditioning = true, WiFi = true, Drinks = true, Snacks = true, ImgURL = "~/uploads/bus1.jpeg" },
                new Bus { Id = 2, Type = "Standard", TV = false, NumberOfSeats = 50, AirConditioning = true, WiFi = false, Drinks = false, Snacks = false, ImgURL = "~/uploads/bus2.jpeg" },
                new Bus { Id = 3, Type = "Comfort", TV = true, NumberOfSeats =   30, AirConditioning = true, WiFi = false, Drinks = true, Snacks = true, ImgURL = "~/uploads/bus3.jpeg" },
                new Bus { Id = 4, Type = "Premium", TV = true, NumberOfSeats =   20, AirConditioning = true, WiFi = true, Drinks = true, Snacks = false, ImgURL = "~/uploads/bus4.jpeg" }
            );

            modelBuilder.Entity<Trip>().HasData(
               new Trip { Id = 1, BusId = 1, FromStationId = 1, Price = 300, NumberOfSeats = 10, AvailableSeats =  10, TOStationId = 3, TripDate = new DateTime(2025, 3, 14, 10, 00, 00) }, // March 14, 2025, 10:00 AM
                new Trip { Id = 2, BusId = 2, FromStationId = 3, Price = 300, NumberOfSeats = 50, AvailableSeats = 50, TOStationId = 5, TripDate = new DateTime(2025, 3, 15, 12, 30, 00) }, // March 15, 2025, 12:30 PM
                new Trip { Id = 3, BusId = 3, FromStationId = 2, Price = 300, NumberOfSeats = 30, AvailableSeats = 30, TOStationId = 4, TripDate = new DateTime(2025, 3, 16, 14, 45, 00) }, // March 16, 2025, 2:45 PM
                new Trip { Id = 4, BusId = 4, FromStationId = 5, Price = 300, NumberOfSeats = 20, AvailableSeats = 20, TOStationId = 1, TripDate = new DateTime(2025, 3, 17, 16, 00, 00) }  // March 17, 2025, 4:00 PM
            );
        }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}
