using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport_system_prototype.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [ForeignKey("vehicle")]
        public int vehicleId { get; set;}
        //added 
        public int Price { get; set; }
        public int NumberOfSeats { get; set; }
        public int AvailableSeats { get; set; }
        //--------------------------------

        [ForeignKey("TOStation")]
        public int TOStationId { get; set;}
        [ForeignKey("FromStation")]
        public int FromStationId { get; set;}

        public DateTime TripDate { get; set; }
        public vehicle ? vehicle { get; set; }
        public Station? TOStation { get; set; }
        public Station? FromStation { get; set; }
    }
}
