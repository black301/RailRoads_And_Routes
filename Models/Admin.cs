using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Transport__system_prototype.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User User { get; set; }
    }
}
