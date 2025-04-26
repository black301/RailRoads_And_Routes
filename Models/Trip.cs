using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transport__system_prototype.Models;

namespace Transport_system_prototype.Models
{
    public class Trip
    {
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

        public virtual Vehicle? vehicle { get; set; }
        public virtual Station? TOStation { get; set; }
        public virtual Station? FromStation { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }

        [NotMapped]
        public virtual IEnumerable<Client> Clients => Bookings?.Select(b => b.Client).Where(c => c != null)! ?? Enumerable.Empty<Client>();
    }

}
