using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }    
        public string ? ProfilePicture { get; set; }
    }
}
