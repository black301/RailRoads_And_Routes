using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.Models
{
    public class Client
    {
<<<<<<< HEAD
        public string Id { get; set; }
=======
        [Key]
        public int Id { get; set; }
>>>>>>> 326099a89b7633b5e8161b733ad6a465c59bf5c7

        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual AppUser User { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }

        [NotMapped]
        public virtual IEnumerable<Trip> Trips => Bookings?.Select(b => b.Trip).Where(t => t != null)! ?? Enumerable.Empty<Trip>();
    }

}
