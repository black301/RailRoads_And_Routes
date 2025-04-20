using System.ComponentModel.DataAnnotations.Schema;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Models
{
    public class Booking 
    {
        public int Id { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ForeignKey("Trip")]
        public int TripId { get; set; }
        public Client? Client { get; set; }
        public Trip? Trip { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfSeats { get; set; }
        [NotMapped]
        public int ComputedTotalPrice => NumberOfSeats * (Trip?.Price ?? 0);
    }
}
