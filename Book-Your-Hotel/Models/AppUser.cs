using Microsoft.AspNetCore.Identity;

namespace Book_Your_Hotel.Models
{
    public class AppUser: IdentityUser
    {
        //BuildIn identity user has not prop named Name.
        public string Name { get; set; }
    }
}
