using System.ComponentModel.DataAnnotations.Schema;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Models
{
    public class Client
    {
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }

        [NotMapped]
        public virtual IEnumerable<Trip> Trips => Bookings?.Select(b => b.Trip).Where(t => t != null)! ?? Enumerable.Empty<Trip>();
    }

}
