using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport_system_prototype.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int NumberOfSeats { get; set; }
        
        public bool TV { get; set; }
        public bool AirConditioning { get; set; }
        public bool WiFi { get; set; }
        public bool Drinks { get; set; }
        public bool Snacks { get; set; }
        public string? ImgURL { get; set; }
        [NotMapped]       
        public  IFormFile? ImageFile { get; set; } // For file upload
    }
}
