using System.ComponentModel.DataAnnotations.Schema;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Models
{
    public class Client
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public User User { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        [NotMapped]
        public IEnumerable<Trip> Trips => Bookings?.Select(b => b.Trip).Where(t => t != null)! ?? Enumerable.Empty<Trip>();
    }

}
