using Microsoft.AspNetCore.Identity;

namespace Transport__system_prototype.Models
{
    public class User : IdentityUser
    {
        public Client? Client { get; set; }
        public Admin ? Admin { get; set; }
        public string City { get; set; }

    }
}
