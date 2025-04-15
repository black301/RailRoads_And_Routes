using System.ComponentModel.DataAnnotations.Schema;

namespace Transport__system_prototype.Models
{
    public class Client
    {
        public  int Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
