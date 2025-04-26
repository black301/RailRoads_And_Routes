using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport__system_prototype.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
