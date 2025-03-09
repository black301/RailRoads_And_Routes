using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_system_prototype.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [ForeignKey("Bus")]
        public int BusId { get; set;}
        [ForeignKey("TOStation")]
        public int TOStationId { get; set;}
        [ForeignKey("FromStation")]
        public int FromStationId { get; set;}

        public DateTime TripDate { get; set; }
        public Bus ? Bus { get; set; }
        public Station? TOStation { get; set; }
        public Station? FromStation { get; set; }
    }
}
