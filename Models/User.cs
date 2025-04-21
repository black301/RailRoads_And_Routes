using Microsoft.AspNetCore.Identity;

namespace Transport__system_prototype.Models
{
    public class User : IdentityUser
    {
        public virtual Client? Client { get; set; }
        public virtual Admin ? Admin { get; set; }
        public string City { get; set; }

    }
}
