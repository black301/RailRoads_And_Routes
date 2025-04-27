using Microsoft.AspNetCore.Identity;

namespace Transport__system_prototype.Models
{
    public class AppUser : IdentityUser
    {
        public virtual Client? Client { get; set; }
        public virtual Admin ? Admin { get; set; }
        public string City { get; set; }
        public string? FullName { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }

    }
}
