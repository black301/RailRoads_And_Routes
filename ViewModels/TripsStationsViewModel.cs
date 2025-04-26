using Transport_system_prototype.Models;

namespace Transport__system_prototype.ViewModels
{
    public class TripsStationsViewModel
    {
        public virtual List<Station> Stations { get; set; }
        public virtual List<Trip> Trips { get; set; }
    }
}
