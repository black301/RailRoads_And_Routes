using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transport__system_prototype.Models;

namespace Transport_system_prototype.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("vehicle")]
        public int vehicleId { get; set; }

        public int Price { get; set; }
        public int NumberOfSeats { get; set; }
        public int AvailableSeats { get; set; }

        [ForeignKey("TOStation")]
        public int TOStationId { get; set; }

        [ForeignKey("FromStation")]
        public int FromStationId { get; set; }

        public DateTime TripDate { get; set; }

        public vehicle? vehicle { get; set; }
        public Station? TOStation { get; set; }
        public Station? FromStation { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        [NotMapped]
        public IEnumerable<Client> Clients => Bookings?.Select(b => b.Client).Where(c => c != null)! ?? Enumerable.Empty<Client>();
    }

}
