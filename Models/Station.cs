using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport_system_prototype.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string? ImgURL { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; } // For file upload
    }
}
