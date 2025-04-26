using System.ComponentModel.DataAnnotations;

namespace Transport_system_prototype.Models
{
    public class vehicle
    {
        [Key]
        public int Id { get; set; }
        //added
        public string Name { get; set; }
        public string Type { get; set; }
        public int NumberOfSeats { get; set; }
        //--------------------------------
        public bool TV { get; set; }
        public bool AirConditioning { get; set; }
        public bool WiFi { get; set; }
        public bool Drinks { get; set; }
        public bool Snacks { get; set; }
        public string ImgURL { get; set; }
    }
}
