using Microsoft.AspNetCore.Identity;

namespace Trustesse_Assessment.Model
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
