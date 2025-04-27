using Transport__system_prototype.Models;
using Transport_system_prototype.Models;

namespace Transport__system_prototype.ViewModels
{
    public class UserTripsStationsViewModel
    {
        public virtual List<Station> Stations { get; set; }
        public virtual List<Trip> Trips { get; set; }
        public virtual List<Vehicle> Vehicles { get; set; }
        public virtual AppUser? AppUser { get; set; }
    }
}
