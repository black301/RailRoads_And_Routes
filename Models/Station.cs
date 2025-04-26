using System.ComponentModel.DataAnnotations;

namespace Transport_system_prototype.Models
{
    public class Station
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImgURL { get; set; }
    }
}
